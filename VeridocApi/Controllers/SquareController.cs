using Microsoft.AspNetCore.Mvc;
using App.Bal.Services;
using App.Entity.Models.SignUp;
using App.Foundation.Common;
using Square;
using Square.Models;
using App.Foundation.Services;
using App.Entity.Models.Plan;
using App.Entity.Dto;
using App.Entity.Dto.Square;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using App.Entity.Models;
using System.Globalization;
using App.Entity.Dto.MainApp;
using App.Entity.Models.MainApp;
using System.Net;

namespace VeridocApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SquareController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;
        private readonly IMailService _mailService;

        public SquareController(IUserService userService, IPaymentService paymentService, IMailService mailService)
        {
            _paymentService = paymentService;
            _userService = userService;
            _mailService = mailService;
        }


        [HttpPost("create-subscription")]
        public async Task<IActionResult> CreateSubscription([FromBody] SubscriptionDto responseDto)
        {
            try
            {
                RegisterUser? appUser = await _userService.FindByEmailAsync(responseDto.CustomerEmail);
                if (appUser == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.SubscriptionError + ", " + ErrorMessages.UserInvalid });
                }
                PlanInfo? planInfo = await _paymentService.GetPlanInfo(appUser.PlanInfoId);
                if (planInfo == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.PlanError });
                }
                CreateCardResponse? cardResponse = await _paymentService.CreateCard(responseDto.CustomerId, responseDto.CardToken, responseDto.HolderName, responseDto.Address, responseDto.State, responseDto.Zip);
                if (cardResponse == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.InvalidCardData });
                }

                CreateSubscriptionResponse? subscriptionResponse = await _paymentService.CreateSubscription(responseDto.PlanId, responseDto.CustomerId, cardResponse.Card.Id);
                if (subscriptionResponse == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.SubscriptionError });
                }

                appUser.CardId = cardResponse.Card.Id;
                appUser.CardToken = responseDto.CardToken;
                appUser.SquareSubscriptionId = subscriptionResponse.Subscription.Id;
                appUser.PaymentStatus = _paymentService.PaymentStatusSuccess;
                int id = await _userService.UpdateAsync(appUser);

                _mailService.SendSignUpMail(new App.Entity.Models.Mail.SignUpMail() 
                { 
                    Email = appUser.Email!,
                    FullName = appUser.FirstName + " " + appUser.LastName, 
                    Subject = "Email Confirmation" 
                });

                return Ok(responseDto);
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.SubscriptionError);
            }
        }



        #region webhook

        [HttpPost("webhook/paymentconfirmation")]
        public async Task<IActionResult> PaymentSuccess() 
        {
            try
            {
                StreamReader streamReader = new (Request.Body);
                string res = await streamReader.ReadToEndAsync();
                PaymentMadeWebhook? paymentMade = JsonConvert.DeserializeObject<PaymentMadeWebhook>(res);

                if (paymentMade == null)
                    return StatusCode((int)HttpStatusCode.NotFound);

                var invoice = paymentMade.data?.@object?.invoice;
                if (invoice == null)
                    return StatusCode((int)HttpStatusCode.NotFound);

                var paymentStatus = invoice.status!.Equals("PAID");

                bool sta = await UpdateAppUser(invoice.subscription_id!, invoice.primary_recipient?.customer_id!, paymentStatus);
                if (sta)
                {
                    return Ok();
                }
                return BadRequest("User not exist");
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { ErrorMessage = e.Message, InnerErrorMessage = e.InnerException?.Message, StackTrace = e.StackTrace });
            }
        }


        [HttpPost("webhook/paymentfailure")]
        public async Task<IActionResult> PaymentFailure()
        {
            try
            {
                StreamReader streamReader = new StreamReader(Request.Body);
                string res = await streamReader.ReadToEndAsync();
                PaymentMadeWebhook? paymentMade = JsonConvert.DeserializeObject<PaymentMadeWebhook>(res);

                if (paymentMade == null)
                    return StatusCode((int)HttpStatusCode.NotFound);

                var invoice = paymentMade?.data?.@object?.invoice;
                if (invoice == null)
                    return StatusCode((int)HttpStatusCode.NotFound);

                AppUser? appUser = await _userService.FindAppUserByCustomerId(invoice.primary_recipient?.customer_id!);
                if (appUser != null)
                {
                    appUser.PaymentStatus = _paymentService.PaymentStatusFail;
                    await _userService.UpdateAppUserAsync(appUser);
                    App.Entity.Models.Plan.Subscription? subscription = await _paymentService.GetSubscription(invoice.primary_recipient?.customer_id!);
                    if (subscription != null)
                    {
                        subscription.Status = _paymentService.SubscriptionStatusFail;
                        subscription.IsActive = false;
                        subscription.Certificates = 0;
                        await _paymentService.UpdateSubscription(subscription);

                    }
                }
                _userService.DisableOrganisation(invoice.primary_recipient?.customer_id!);
                var amount = (invoice.payment_requests![0].computed_amount_money!.amount / 100).ToString();

                _mailService.SendPaymentFail(new App.Entity.Models.Mail.SignUpMail() 
                { 
                    Email = invoice.primary_recipient?.email_address!,
                    Amount = amount,
                    PackName = "", 
                    RepayLink = invoice.public_url ?? "",
                    Subject = "Payment Failure" 
                });

                return Ok("");
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { ErrorMessage = e.Message, InnerErrorMessage = e.InnerException?.Message, StackTrace = e.StackTrace });
            }
        }


        [HttpPost("webhook/notify")]
        public async Task<ActionResult> NotificationFromSquare()
        {
            try
            {
                var isFromSquare = await _paymentService.ValidateRequest(Request);

                if (isFromSquare)
                    return Ok();
                else
                    return StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }


        [HttpPost("webhook/paymentupdate")]
        public async Task<IActionResult> UpdatePayment()
        {
            try
            {
                StreamReader streamReader = new StreamReader(Request.Body);
                string res = await streamReader.ReadToEndAsync();
                PaymentUpdateWebhook? paymentMade = JsonConvert.DeserializeObject<PaymentUpdateWebhook>(res);
                if (paymentMade is not  null && paymentMade?.PaymentData?.PaymentObject?.Payment?.Status == "COMPLETED")
                {
                    TransectionHistory? transectionHistory = await _paymentService.GetTransectionHistoryByPaymentId(paymentMade?.PaymentData?.PaymentObject?.Payment?.Id);
                    if (transectionHistory is not null)
                    {
                        transectionHistory.PaymentStatus = _paymentService.PaymentStatusSuccess;
                        await _paymentService.UpdateTransectionHistory(transectionHistory);
                        _userService.UpdateTemplateRequest(transectionHistory.CustomerId, transectionHistory.TemplateQty);
                        return Ok();
                    }
                }
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { ErrorMessage = e.Message, InnerErrorMessage = e.InnerException?.Message, StackTrace = e.StackTrace });
            }
        }


        private async Task<bool> UpdateAppUser(string subscriptionId, string customerId, bool paymentStatus)
        {
            RegisterUser? registerUser = await _userService.FindByCustomerId(customerId);
            if (registerUser != null)
            {
                if (!registerUser.IsActive && paymentStatus)
                {
                    string password = Utils.GeneratePassword(6);
                    string organisationName = string.IsNullOrEmpty(registerUser.CompanyName) ? StorageHelper.GetOrganisationName(registerUser.Email) : registerUser.CompanyName;
                    string organisationAddress = string.Empty;
                    if (!string.IsNullOrEmpty(registerUser.Address))
                    {
                        organisationAddress = registerUser.Address + ",";
                    }
                    if (!string.IsNullOrEmpty(registerUser.City)) { organisationAddress += registerUser.City + ","; }
                    if (!string.IsNullOrEmpty(registerUser.State)) { organisationAddress += registerUser.State + ","; }
                    if (!string.IsNullOrEmpty(registerUser.Country)) { organisationAddress += registerUser.Country + ","; }
                    int certificates = registerUser?.PlanInfo?.Timespan.ToLower() == "monthly" ? registerUser.PlanInfo.Certificates : registerUser.PlanInfo!.Certificates;
                    OrganisationDeatailsDto deatailsDto = new()
                    {
                        OrganisationName = organisationName,
                        AdminFirstName = registerUser.FirstName,
                        AdminLastName = registerUser.LastName,
                        AdminPassword = Encryption.EncryptPassword(password),
                        ContactEmail = registerUser.Email,
                        CountryCodeId = registerUser.CountryId,
                        ContactNumber = registerUser.PhoneNumber,
                        SquareCustomerId = customerId,
                        CountryName = registerUser.Country,
                        State = registerUser.State,
                        City = registerUser.City,
                        ZipCode = registerUser.Zip,
                        BillingCompanyName = registerUser.BillingCompanyName,
                        BillingCompanyAddress = registerUser.BillingAddress,
                        BillingCompanyCountry = registerUser.BillingCountryId,
                        BillingCompanyState = registerUser.BillingState,
                        BillingCompanyCity = registerUser.BillingCity,
                        BillingCompanyZip = registerUser.BillingZip,
                        OrganisationAddress = organisationAddress.Trim(','),
                        SubscriptionCounter = certificates,
                        OrganisationShortName = StorageHelper.CreateOrganisationBucketName(StorageHelper.GetOrganisationName(organisationName))[..3].ToUpper(),
                        PlanId = registerUser.MainAppPlanId,
                        IsBillingSame = registerUser.IsBillingSame,
                    };
                    int creationSuccessfull = await _userService.AddOrganisation(deatailsDto);
                    if (creationSuccessfull == 1)
                    {

                        AppUser appUser = new()
                        {
                            Password = password,
                            FirstName = registerUser.FirstName,
                            LastName = registerUser.LastName,
                            Email = registerUser.Email,
                            PhoneCode = registerUser.PhoneCode,
                            PhoneNumber = registerUser.PhoneNumber,
                            CompanyName = registerUser.CompanyName,
                            Address = registerUser.Address,
                            City = registerUser.City,
                            State = registerUser.State,
                            Country = registerUser.Country,
                            Zip = registerUser.Zip,
                            BillingCompanyName = registerUser.BillingCompanyName,
                            BillingAddress = registerUser.BillingAddress,
                            BillingCity = registerUser.BillingCity,
                            BillingState = registerUser.BillingState,
                            BillingCountry = registerUser.BillingCountry,
                            BillingZip = registerUser.BillingZip,
                            CustomerId = customerId,
                            CardId = registerUser.CardId,
                            CardToken = registerUser.CardToken,
                            SquareSubscriptionId = subscriptionId,
                            Plan = registerUser.Plan,
                            PlanId = registerUser.PlanId,
                            PlanInfoId = registerUser.PlanInfoId,
                            PlanTimeSpan = registerUser.PlanTimeSpan,
                            IsActive = true,
                            PaymentStatus = _paymentService.PaymentStatusSuccess
                        };

                        int id = await _userService.CreateAppUserAsync(appUser);

                        int month = appUser.PlanTimeSpan?.ToLower() == "yearly" ? 12 : 1;
                        PlanInfo? planInfo = await _paymentService.GetPlanInfo(appUser.PlanInfoId);


                        App.Entity.Models.Plan.Subscription subscription = new()
                        {
                            AppUserId = id,
                            StartTime = DateTime.UtcNow,
                            EndTime = DateTime.UtcNow.AddMonths(month),
                            Timespan = appUser.PlanTimeSpan,
                            Certificates = certificates,
                            CustomerId = customerId,
                            SubscriptionId = subscriptionId,
                            CardId = registerUser.CardId,
                            PlanInfoId = appUser.PlanInfoId,
                            Status = _paymentService.SubscriptionStatusSuccess,
                            IsActive = true,
                            CurrentMonth = 1,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        await _userService.CreateSubscriptionAsync(subscription);

                        SubscriptionHistory subscriptionHistory = new()
                        {
                            AppUserId = id,
                            CustomerId = customerId,
                            PlanName = planInfo.PlanName,
                            TimeSpan = appUser.PlanTimeSpan,
                            Certificates = certificates,
                            SubscriptionId = subscriptionId,
                            Price = planInfo.Timespan.ToLower() == "monthly" ? planInfo.Price : planInfo.Price * 12,
                            StartTime = subscription.StartTime,
                            EndTime = subscription.EndTime,
                            Status = "ACTIVE"
                        };

                        await _paymentService.CreateSubscriptionHistory(subscriptionHistory);

                        registerUser.IsActive = true;
                        registerUser.SquareSubscriptionId = subscriptionId;
                        await _userService.UpdateAsync(registerUser);

                        if (registerUser.IsTemplate)
                        {
                            string refid = Guid.NewGuid().ToString();
                            double aud = await CurrencyConvertor.GetAUD();
                            decimal AUDPrice = 90 * Convert.ToDecimal(aud);
                            CreatePaymentResponse? response = await _paymentService.CreatePayment(AUDPrice, registerUser.CustomerId!, refid, registerUser.CardId!);
                            if (response is not null)
                            {
                                TransectionHistory transectionHistory = new()
                                {
                                    Email = registerUser.Email!,
                                    RefId = refid,
                                    PaymentType = _paymentService.PaymentTypePayment,
                                    CreateAt = DateTime.UtcNow,
                                    PaymentId = response.Payment.Id,
                                    PaymentStatus = _paymentService.PaymentStatusPending,
                                    CustomerId = registerUser.CustomerId!,
                                    TemplateQty = 1
                                };
                                await _paymentService.CreateTransectionHistory(transectionHistory);
                            }
                        }


                        _mailService.SendUserCredential(new App.Entity.Models.Mail.SignUpMail() { Email = appUser.Email!, FirstName = appUser.FirstName ?? "", Password = password, Subject = "User Info" });
                    }
                }
                else
                {
                    if (paymentStatus)
                    {
                        App.Entity.Models.Plan.Subscription subscription = await _paymentService.GetSubscription(customerId);
                        AppUser? appUser = await _userService.FindAppUserById(subscription.AppUserId);
                        appUser.SquareSubscriptionId = subscriptionId;
                        appUser.CustomerId = customerId;
                        appUser.PaymentStatus = _paymentService.PaymentStatusSuccess;
                        await _userService.UpdateAppUserAsync(appUser);
                        int previousCertificates = subscription.Certificates;
                        
                        int certificates = subscription.PlanInfo!.Certificates;

                        subscription.StartTime = DateTime.UtcNow;
                        subscription.SubscriptionId = subscriptionId;
                        int month = subscription.PlanInfo.Timespan.ToLower() == "yearly" ? 12 : 1;
                        subscription.EndTime = DateTime.UtcNow.AddMonths(month);
                        subscription.Timespan = appUser.PlanTimeSpan;
                        subscription.Certificates = previousCertificates + certificates;
                        subscription.CustomerId = customerId;
                        subscription.Status = _paymentService.SubscriptionStatusSuccess;
                        subscription.UpdatedAt = DateTime.UtcNow;
                        subscription.IsActive = true;
                        subscription.CurrentMonth = 1;

                        SubscriptionHistory subscriptionHistory = new()
                        {
                            AppUserId = appUser.Id,
                            CustomerId = customerId,
                            SubscriptionId = subscriptionId,
                            PlanName = subscription.PlanInfo.PlanName,
                            TimeSpan = subscription.PlanInfo.Timespan,
                            Certificates = subscription.Certificates,
                            Price = subscription.PlanInfo.Price,
                            StartTime = subscription.StartTime,
                            EndTime = subscription.EndTime,
                            Status = "ACTIVE"
                        };

                        if (subscription.PlanInfo.PlanName.ToLower() == "free")
                        {
                            PlanInfo? planInfo = await _paymentService.GetPlanInfo(appUser.PlanInfoId);
                            certificates = planInfo!.Certificates;
                            month = planInfo.Timespan.ToLower() == "yearly" ? 12 : 1;
                            subscription.StartTime = DateTime.UtcNow;
                            subscription.EndTime = DateTime.UtcNow.AddMonths(month);
                            subscription.Certificates = previousCertificates + certificates;
                            subscription.PlanInfoId = planInfo.Id;
                            subscription.Timespan = planInfo.Timespan;
                            subscription.CardId = appUser.CardId;
                            subscription.CurrentMonth = 1;

                            subscriptionHistory.PlanName = planInfo.PlanName;
                            subscriptionHistory.TimeSpan = planInfo.Timespan;
                            subscriptionHistory.Certificates = subscription.Certificates;
                            subscriptionHistory.Price = planInfo.Timespan.ToLower() == "monthly" ? planInfo.Price : planInfo.Price * 12;
                        }


                        await _paymentService.UpdateSubscription(subscription);
                        await _paymentService.CreateSubscriptionHistory(subscriptionHistory);
                        _userService.UpdateBalance(certificates, customerId);
                    }
                }
                return true;
            }
            return false;
        }

        #endregion


        #region user api

        [HttpGet("getcurrentactiveplan")]
        public async Task<IActionResult> GetCurrentActivePlan(string squareCustomerID)
        {
            try
            {
                App.Entity.Models.Plan.Subscription? subscription1 = await _paymentService.GetSubscription(squareCustomerID);
                if (subscription1 == null)
                {
                    return BadRequest(new { error = "Invalid Customer ID" });
                }
                if (string.IsNullOrEmpty(subscription1.SubscriptionId))
                {
                    PlanInfoResponseDto planInfo = new()
                    {
                        PlanName = subscription1.PlanInfo?.PlanName,
                        Pricing = subscription1.PlanInfo!.Price,
                        Description = subscription1.PlanInfo.Description,
                        Cadence = subscription1.PlanInfo.Timespan,
                        RegisterDate = subscription1.StartTime.ToString("yyyy-MM-dd"),
                        ExpiryDate = subscription1.EndTime.ToString("yyyy-MM-dd"),
                        Status = subscription1.IsActive ? "ACTIVE" : "INACTIVE",
                        Certificate = subscription1.PlanInfo.Certificates
                    };
                    return Ok(planInfo);
                }
                RetrieveCustomerResponse? customerResponse = await _paymentService.RetrieveCustomer(squareCustomerID);
                if (customerResponse == null)
                {
                    return BadRequest(new { error = "Invalid Customer ID" });
                }
                RetrieveSubscriptionResponse subscriptionResponse = await _paymentService.RetrieveSubscription(subscription1.SubscriptionId);
                if (subscriptionResponse == null)
                {
                    return BadRequest(new { error = "Invalid Customer ID" });
                }
                PlanInfoResponseDto planInfoResponseDto = new ();
                planInfoResponseDto.PlanName = subscription1.PlanInfo?.PlanName;
                planInfoResponseDto.Pricing = subscription1.PlanInfo!.Price;
                planInfoResponseDto.Description = subscription1.PlanInfo.Description;
                planInfoResponseDto.Cadence = subscription1.PlanInfo.Timespan;
                planInfoResponseDto.CardType = customerResponse.Customer.Cards.FirstOrDefault()?.CardBrand;
                planInfoResponseDto.CardDigit = customerResponse.Customer.Cards.FirstOrDefault()?.Last4;
                planInfoResponseDto.RegisterDate = subscription1.StartTime.ToString("yyyy-MM-dd");
                planInfoResponseDto.ExpiryDate = subscription1.EndTime.ToString("yyyy-MM-dd");
                planInfoResponseDto.NextBill = subscription1.EndTime.AddDays(1).ToString("yyyy-MM-dd");
                planInfoResponseDto.BillPeriod = subscriptionResponse.Subscription.ChargedThroughDate;
                planInfoResponseDto.Status = subscription1.IsActive ? "ACTIVE" : "INACTIVE";
                planInfoResponseDto.Certificate = subscription1.PlanInfo.Certificates;

                return Ok(planInfoResponseDto);
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { ErrorMessage = e.Message, InnerErrorMessage = e.InnerException?.Message, StackTrace = e.StackTrace });
            }
        }


        [HttpGet("getbillinghistory")]
        public async Task<IActionResult> GetBillingHistory(string squareCustomerID)
        {
            try
            {
                List<SubscriptionHistory> subscriptionHistories = await _paymentService.GetSubscriptionHistory(squareCustomerID);

                List<SquareBillingDataResponse> billing = new();
                List<PlanInfo> planInfos = await _paymentService.GetPlans();

                for (var transcount = 0; transcount < subscriptionHistories.Count; transcount++)
                {
                    if (string.IsNullOrEmpty(subscriptionHistories[transcount].SubscriptionId))
                    {
                        billing.Add(new SquareBillingDataResponse
                        {
                            billingDate = subscriptionHistories[transcount].StartTime.ToLongDateString(),
                            squareInvoiceLink = "",
                            servicePeriod = "1 Month",
                            cardType = "",
                            cardDigits = "",
                            amount = "0",
                            planDescription = planInfos.FirstOrDefault(e => e.PlanName.ToLower() == "free")?.Description
                        });
                    }
                    else
                    {
                        var subscription = await _paymentService.RetrieveSubscription(subscriptionHistories[transcount].SubscriptionId);
                        if (subscription.Errors != null)
                            return Ok(new { error = "Invalid Customer ID" });

                        for (int i = 0; i < subscription.Subscription.InvoiceIds.Count; i++)
                        {
                            var invoice = await _paymentService.GetInvoice(subscription.Subscription.InvoiceIds[i]);

                            var cardID = invoice.Invoice.PaymentRequests[0].CardId;

                            var card = await _paymentService.RetrieveCard(string.IsNullOrEmpty(cardID) ? subscription.Subscription.CardId : cardID);
                            if (card.Errors != null)
                                return Ok(new { error = "Invalid Card Data" });

                            var amount = (decimal?)invoice.Invoice.PaymentRequests[0].ComputedAmountMoney.Amount / 100;
                            var order = await _paymentService.RetrieveOrder(invoice.Invoice.OrderId);
                            var orderNote = order.Order.LineItems[0].Note;

                            billing.Add(new SquareBillingDataResponse
                            {
                                billingDate = invoice.Invoice.CreatedAt,
                                squareInvoiceLink = invoice.Invoice.PublicUrl,
                                servicePeriod = orderNote,
                                cardType = card.Card.CardBrand,
                                cardDigits = string.IsNullOrEmpty(cardID) ? "N/a" : card.Card.Last4,
                                amount = $"{amount}",
                                planDescription = planInfos.FirstOrDefault(e => e.PlanName.ToLower() == subscriptionHistories[transcount].PlanName.ToLower() && e.Timespan.ToLower() == subscriptionHistories[transcount].TimeSpan.ToLower())?.Description
                            });
                        }
                    }
                }

                return Ok(billing.OrderByDescending(e => e.billingDate).ToList());
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { ErrorMessage = e.Message, InnerErrorMessage = e.InnerException?.Message, StackTrace = e.StackTrace });
            }
        }


        [HttpPost("cancelplan")]
        public async Task<IActionResult> CancelPlan([FromBody]UserApiDto userApi)
        {
            try
            {
                App.Entity.Models.Plan.Subscription subscription = await _paymentService.GetSubscription(userApi.CustomerId);
                if (subscription == null)
                {
                    return BadRequest(new { message = "Invalid customer id" });
                }
                if (string.IsNullOrEmpty(subscription.SubscriptionId) || subscription?.PlanInfo?.PlanName.ToLower() == "free")
                {
                    return Ok(new { message = "Your are currently in free plan." });
                }
                var response = await _paymentService.CancelSubscription(subscription!.SubscriptionId);
                if (response == null)
                {
                    return BadRequest(new { message = "Unable to cancel plan" });
                }
                if (response.Errors != null)
                {
                    return BadRequest(new { error = response.Errors });
                }
                if (DateTime.TryParseExact(response.Subscription.CanceledDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    if (!string.IsNullOrEmpty(subscription.CardId))
                    {
                         DisableCardResponse cardResponse = await _paymentService.DisableCard(subscription.CardId);
                        if(cardResponse != null)
                        {
                            subscription.CardId = null;
                        }
                    }
                    subscription.IsActive = false;
                    subscription.Status = "CANCELED";
                    subscription.SubscriptionId = null;
                    await _paymentService.UpdateSubscription(subscription);
                    _userService.DisableOrganisation(userApi.CustomerId);
                    _mailService.SendPlanCancellation(userApi);
                    return Ok(new { CanceledDate = dateTime.ToString("yyyy-MM-dd") });
                }
                else
                    return BadRequest(new { error = response.Errors });
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { ErrorMessage = e.Message, InnerErrorMessage = e.InnerException?.Message, StackTrace = e.StackTrace });
            }
        }


        [HttpGet("updatecarddetail")]
        public async Task<IActionResult> UpdateCardDetials([FromQuery] string customerId)
        {
            RetrieveCustomerResponse? response = await _paymentService.RetrieveCustomer(customerId);
            AppUser? appUser = await _userService.FindAppUserByCustomerId(customerId);
            if (response == null || appUser == null)
            {
                return BadRequest(new App.Entity.Models.HttpResponse() { Content = "Invalid Customer Id", StatusCode = 400 });
            }
            string payurl = $"https://stage.veridoccertificate.com/payment/card?uid={customerId}";
            return Ok(new { url = payurl, appId = _paymentService.SquareConfig.ApplicationId, locationId = _paymentService.SquareConfig.LocationId, email = appUser.Email, name = appUser.FirstName + " " + appUser.LastName });
        }


        [HttpPost("submitcarddetails")]
        public async Task<IActionResult> SubmitCardDetails([FromBody] CardDetailDto detailDto)
        {
            try
            {
                AppUser? appUser = await _userService.FindAppUserByCustomerId(detailDto.CustomerId);
                if (appUser == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.UserInvalid });
                }
                RetrieveCustomerResponse? customerResponse = await _paymentService.RetrieveCustomer(detailDto.CustomerId);
                if (customerResponse == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.UserInvalid });
                }
                DisableCardResponse disableCardResponse = await _paymentService.DisableCard(appUser.CardId!);
                if (disableCardResponse == null || disableCardResponse.Errors != null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.CardUpdateError });
                }
                CreateCardResponse? createCardResponse = await _paymentService.CreateCard(detailDto.CustomerId, detailDto.CardToken, detailDto.HolderName, detailDto.Address, detailDto.State, detailDto.Zip);
                if (createCardResponse == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.InvalidCardData });
                }
                appUser.CardId = createCardResponse.Card.Id;
                appUser.CardToken = detailDto.CardToken;
                await _userService.UpdateAppUserAsync(appUser);
                RegisterUser? registerUser = await _userService.FindByCustomerId(detailDto.CustomerId);
                if (registerUser != null)
                {
                    registerUser.CardId = createCardResponse.Card.Id;
                    registerUser.CardToken = detailDto.CardToken;
                    await _userService.UpdateAsync(registerUser);
                }
                App.Entity.Models.Plan.Subscription subscription = await _paymentService.GetSubscription(detailDto.CustomerId);
                if (subscription != null)
                {
                    subscription.CardId = createCardResponse.Card.Id;
                    await _paymentService.UpdateSubscription(subscription);
                }

                return Ok(new App.Entity.Models.HttpResponse() { StatusCode = 200, Content = "Card is updated", IsSuccess = true });
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { ErrorMessage = e.Message, InnerErrorMessage = e.InnerException?.Message, StackTrace = e.StackTrace });
            }
        }


        [HttpPost]
        [Route("updateplan")]
        public async Task<IActionResult> UpdatePlan([FromBody] PlanDetailsDto parameters)
        {
            try
            {
                List<OrganisationPlan> organisationPlans = _userService.GetOrganisationPlans();
                if (!organisationPlans.Any(e => e.plan_name.ToLower() == parameters.PlanName.ToLower()))
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { Content = ErrorMessages.PlanError, StatusCode = 400 });
                }
                if (!organisationPlans.Any(e => e.Cadence.ToLower() == parameters.PlanTimespan.ToLower()))
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { Content = ErrorMessages.PlanTimeError, StatusCode = 400 });
                }
                List<PlanInfo> planInfos = await _paymentService.GetPlanInfo(parameters.PlanName);
                if (planInfos.Count == 0)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { Content = ErrorMessages.PlanError, StatusCode = 400 });
                }
                if (!planInfos.Any(e => e.Timespan.ToLower() == parameters.PlanTimespan.ToLower()))
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { Content = ErrorMessages.PlanTimeError, StatusCode = 400 });
                }

                App.Entity.Models.Plan.Subscription? subscription = await _paymentService.GetSubscription(parameters.CustomerId);
                if (subscription == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { Content = ErrorMessages.InvalidCustomerID, StatusCode = 400 });
                }
                if (string.IsNullOrEmpty(subscription.SubscriptionId) || subscription.PlanInfo?.PlanName.ToLower() == "free")
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { Content = "Please Buy a new plan", StatusCode = 400 });
                }
                RetrieveSubscriptionResponse subscriptionResponse = await _paymentService.RetrieveSubscription(subscription.SubscriptionId);
                if (subscriptionResponse == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { Content = ErrorMessages.SubscriptionRetrieveError, StatusCode = 400 });
                }
                if (subscription.IsActive && !string.IsNullOrEmpty(subscription.SubscriptionId))
                {
                    CancelSubscriptionResponse cancelSubscriptionResponse = await _paymentService.CancelSubscription(subscription.SubscriptionId);
                    if (cancelSubscriptionResponse == null)
                    {
                        return BadRequest(new App.Entity.Models.HttpResponse() { Content = "unable to update plan", StatusCode = 400 });
                    }
                }
                int previousPlanid = subscription.PlanInfo!.Id;
                string planId = _paymentService.GetSubscriptionPlanId(planInfos, parameters.PlanTimespan);

                
                subscription.PlanInfoId = parameters.PlanTimespan.ToLower() == "yearly" ? planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "yearly").Id : planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "monthly").Id;
                subscription.UpdatedAt = DateTime.UtcNow;
                await _paymentService.UpdateSubscription(subscription);

                CreateSubscriptionResponse? createSubscriptionResponse = await _paymentService.CreateSubscription(planId, subscription.CustomerId, subscription.CardId);
                if (createSubscriptionResponse == null)
                {
                    subscription.PlanInfoId = previousPlanid;
                    subscription.UpdatedAt = DateTime.UtcNow;
                    await _paymentService.UpdateSubscription(subscription);
                    return BadRequest(new App.Entity.Models.HttpResponse() { Content = "unable to update plan", StatusCode = 400 });
                }
                int certificates = parameters.PlanTimespan.ToLower() == "yearly" ? planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "yearly")!.Certificates: planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "monthly")!.Certificates;
                //int month = parameters.PlanTimespan.ToLower() == "yearly" ? 12 : 1;
                //subscription.Status = _paymentService.SubscriptionStatusSuccess;
                //subscription.CardId = createSubscriptionResponse.Subscription.CardId;
                //subscription.Certificates += certificates;
                //subscription.StartTime = DateTime.UtcNow;
                //subscription.EndTime = DateTime.UtcNow.AddMonths(month);
                //subscription.Timespan = parameters.PlanTimespan;
                //subscription.IsActive = true;


                //SubscriptionHistory subscriptionHistory = new();
                //subscriptionHistory.AppUserId = subscription.AppUserId;
                //subscriptionHistory.CustomerId = subscription.CustomerId;
                //subscriptionHistory.PlanName = planInfos[0].PlanName;
                //subscriptionHistory.TimeSpan = parameters.PlanTimespan;
                //subscriptionHistory.Certificates = certificates;
                //subscriptionHistory.SubscriptionId = createSubscriptionResponse.Subscription.Id;
                //subscriptionHistory.Price = parameters.PlanTimespan.ToLower() == "yearly" ? planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "yearly").Price : planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "monthly").Price;
                //subscriptionHistory.StartTime = subscription.StartTime;
                //subscriptionHistory.EndTime = subscription.EndTime;
                //subscriptionHistory.Status = "ACTIVE";
                //await _paymentService.CreateSubscriptionHistory(subscriptionHistory);

                AppUser? appUser = await _userService.FindAppUserByCustomerId(subscription.CustomerId);
                appUser.PlanInfoId = subscription.PlanInfoId;
                appUser.Plan = planInfos[0].PlanName;
                appUser.PlanTimeSpan = parameters.PlanTimespan;
                appUser.SquareSubscriptionId = createSubscriptionResponse.Subscription.Id;
                appUser.PlanId = planId;
                await _userService.UpdateAppUserAsync(appUser);

                RegisterUser? registerUser = await _userService.FindByCustomerId(subscription.CustomerId);
                registerUser.PlanInfoId = subscription.PlanInfoId;
                registerUser.Plan = planInfos[0].PlanName;
                registerUser.PlanTimeSpan = parameters.PlanTimespan;
                registerUser.SquareSubscriptionId = createSubscriptionResponse.Subscription.Id;
                registerUser.PlanId = planId;
                await _userService.UpdateAsync(registerUser);

                _userService.UpdateOrganisationPlan(subscription.CustomerId, organisationPlans.FirstOrDefault(e => e.plan_name.ToLower() == parameters.PlanName.ToLower() && e.Cadence.ToLower() == parameters.PlanTimespan.ToLower())!.plan_id);
                return Ok(new
                {
                    planName = parameters.PlanName,
                    planPricing = parameters.PlanTimespan.ToLower() == "yearly" ? planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "yearly")?.Price : planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "monthly")?.Price,
                    planDescription = parameters.PlanTimespan.ToLower() == "yearly" ? planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "yearly")?.Description : planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "monthly")?.Description,
                    startDate = subscription.StartTime.ToString("yyyy/MM/dd hh:mm:ss tt"),
                    endDate = subscription.EndTime.ToString("yyyy/MM/dd hh:mm:ss tt")
                });
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { ErrorMessage = e.Message, InnerErrorMessage = e.InnerException?.Message, StackTrace = e.StackTrace });
            }
        }


        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentModel paymentModel)
        {
            try
            {
                App.Entity.Models.Plan.Subscription subscription = await _paymentService.GetSubscription(paymentModel.CustomerId);
                if (subscription is not null)
                {
                    if (subscription.PlanInfo?.PlanName?.ToLower() == "free")
                    {
                        return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.UpgradePlan });
                    }
                    if (string.IsNullOrEmpty(subscription.CardId))
                    {
                        return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.InvalidCard });
                    }
                    decimal price = paymentModel.Certificates * subscription.PlanInfo!.CertificatesPrice;
                    price = price == 0 ? paymentModel.Price : price;
                    if (price == 0) { return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.InvalidPrice }); }
                    double aud = await CurrencyConvertor.GetAUD();
                    decimal AUDPrice = price * Convert.ToDecimal(aud);
                    CreatePaymentResponse? createPayment = await _paymentService.CreatePayment(AUDPrice, subscription.CustomerId, subscription.CardId);
                    if (createPayment is not null && createPayment.Payment.Status == "COMPLETED")
                    {
                        _userService.UpdateBalance(paymentModel.Certificates, paymentModel.CustomerId);
                        return Ok(new App.Entity.Models.HttpResponse() { StatusCode = 200, Content = "Payment Success" });
                    }
                }
                return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.SubscriptionRetrieveError });
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        [HttpPost("buy-template")]
        public async Task<IActionResult> BuyTemplate([FromBody] BuyTemplate buyTemplate)
        {
            try
            {
                if (buyTemplate.Template <= 0)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.InvalidTemplateQty });
                }
                App.Entity.Models.Plan.Subscription subscription = await _paymentService.GetSubscription(buyTemplate.CustomerId);
                if (subscription is not null)
                {
                    if (subscription.PlanInfo?.PlanName?.ToLower() == "free")
                    {
                        return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.UpgradePlan });
                    }
                    if (string.IsNullOrEmpty(subscription.CardId))
                    {
                        return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.InvalidCard });
                    }
                    decimal price = buyTemplate.Template * subscription.PlanInfo!.TemplatePrice;
                    double aud = await CurrencyConvertor.GetAUD();
                    decimal AUDPrice = price * Convert.ToDecimal(aud);
                    string refid = Guid.NewGuid().ToString();
                    CreatePaymentResponse? createPayment = await _paymentService.CreatePayment(AUDPrice, subscription.CustomerId, refid, subscription.CardId);
                    if (createPayment is not null && createPayment.Payment.Status == "COMPLETED")
                    {
                        TransectionHistory transectionHistory = new()
                        {
                            Email = buyTemplate.Email,
                            RefId = refid,
                            PaymentType = _paymentService.PaymentTypePayment,
                            CreateAt = DateTime.UtcNow,
                            PaymentId = createPayment.Payment.Id,
                            PaymentStatus = _paymentService.PaymentStatusPending,
                            CustomerId = buyTemplate.CustomerId,
                            TemplateQty = buyTemplate.Template,
                        };
                        await _paymentService.CreateTransectionHistory(transectionHistory);
                        return Ok(new App.Entity.Models.HttpResponse() { StatusCode = 200, Content = "Payment Success" });
                    }
                }
                return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.SubscriptionRetrieveError });
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        #endregion

    }
}
