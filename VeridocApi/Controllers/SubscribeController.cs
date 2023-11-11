using Microsoft.AspNetCore.Mvc;
using App.Bal.Services;
using App.Entity.Models;
using App.Foundation.Common;
using Square.Models;
using App.Foundation.Services;
using App.Entity.Models.Plan;
using App.Entity.Models.SignUp;
using App.Entity.Models.Mail;
using App.Entity.Dto;
using App.Entity.Dto.MainApp;
using App.Entity.Models.MainApp;

namespace VeridocApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class SubscribeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        public SubscribeController(IUserService userService, IPaymentService paymentService, IMailService mailService, IConfiguration configuration)
        {
            _paymentService = paymentService;
            _userService = userService;
            _mailService = mailService;
            _configuration = configuration;
        }


        [HttpPost("checkuser")]
        public IActionResult CheckUser(string email)
        {
            try
            {
                bool isExist = _userService.FindMainAppUserByEmail(email);
                return Ok(isExist);
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUpUser([FromBody]SignupDto user)
        {
            try
            {
                bool isExist = _userService.FindMainAppUserByEmail(user.Email);
                if (isExist)
                {
                    App.Entity.Models.HttpResponse response = new () { StatusCode = 400, Content = ErrorMessages.UserExists };
                    return BadRequest(response);
                }
                
                List<PlanInfo> planInfos = await _paymentService.GetPlanInfo(user.Plan);
                List<OrganisationPlan> organisationPlans = _userService.GetOrganisationPlans();
                if (planInfos.Count == 0)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.PlanError });
                }
                if (!planInfos.Any(e => e.Timespan.ToLower() == user.PlanTimeSpan.ToLower()))
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.PlanTimeError });
                }
                if (!organisationPlans.Any(e => e.Cadence.ToLower() == user.PlanTimeSpan.ToLower()))
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.PlanTimeError });
                }
                if (!organisationPlans.Any(e => e.plan_name.ToLower() == user.Plan.ToLower()))
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.PlanTimeError });
                }
                user.CompanyName = string.IsNullOrEmpty(user.CompanyName) ? (user.FirstName + " " + user.LastName) : user.CompanyName;
                double aud = await CurrencyConvertor.GetAUD();
                
                decimal USDPrice = user.PlanTimeSpan.ToLower() == "yearly" ? planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "yearly")!.Price * 12 : planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "monthly")!.Price;
                if (user.IsTemplate)
                {
                    USDPrice += 90;
                }
                decimal AUDPrice = USDPrice * Convert.ToDecimal(aud);
                int certificatesCount = user.PlanTimeSpan.ToLower() == "yearly" ? planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "yearly")!.Certificates : planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "monthly")!.Certificates;

                string? planId = _paymentService.GetSubscriptionPlanId(planInfos, user.PlanTimeSpan);

                RegisterUser? registerUser = await _userService.FindByEmailAsync(user.Email);
                if (registerUser != null)
                {
                    registerUser.IsTemplate = user.IsTemplate;
                    registerUser.FirstName = user.FirstName;
                    registerUser.LastName = user.LastName;
                    registerUser.PhoneNumber = user.PhoneNumber;
                    registerUser.CompanyName = user.CompanyName;
                    registerUser.Country = user.Country;
                    registerUser.CountryCode = user.CountryCode;
                    registerUser.PhoneCode = user.PhoneCode;
                    registerUser.Address = user.Address;
                    registerUser.City = user.City;
                    registerUser.State = user.State;
                    registerUser.Zip = user.Zip;
                    registerUser.IsBillingSame = user.IsBiilingSame;
                    registerUser.BillingCompanyName = user.BillingCompanyName;
                    registerUser.BillingAddress = user.BillingAddress;
                    registerUser.BillingCity = user.BillingCity;
                    registerUser.BillingState = user.BillingState;
                    registerUser.BillingZip = user.BillingZip;
                    registerUser.BillingCountry = user.BillingCountry;
                    registerUser.BillingCountryCode = user.BillingCountryCode;
                    registerUser.Plan = user.Plan;
                    registerUser.PlanTimeSpan = user.PlanTimeSpan.ToLower() == "yearly" ? "Yearly" : "Monthly";

                    await _userService.UpdateAsync(registerUser);

                    CustomerResponseDto dto1 = new() 
                    { 
                        CustomerId = registerUser.CustomerId!, 
                        CustomerName = user.FirstName + " " + user.LastName, 
                        PriceAUD = AUDPrice, 
                        PriceUSD = USDPrice,
                        Timespan = user.PlanTimeSpan.ToLower(),
                        Certificates = certificatesCount,
                        CustomerEmail = user.Email, 
                        PlanName = planInfos[0].PlanName,
                        PlanId = planId,
                        AppId = _paymentService.SquareConfig.ApplicationId, 
                        LocationId = _paymentService.SquareConfig.LocationId,
                        IsTemplate = user.IsTemplate
                    };
                    return Ok(dto1);
                }

                List<CountryModel> countryModels = _userService.GetCountryModels();
                RegisterUser appUser = CreateUser(user, countryModels, organisationPlans);
                CreateCustomerResponse? customerResponse = await _paymentService.CreateCsutomer(appUser);
                if (customerResponse == null || customerResponse.Errors != null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.UserCreationFail });
                }
                
                appUser.CustomerId = customerResponse.Customer.Id;
                appUser.PlanInfoId = user.PlanTimeSpan.ToLower() == "yearly" ? planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "yearly")!.Id : planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "monthly")!.Id;
                appUser.PlanId = planId;
                int key = await _userService.CreateAsync(appUser);
                CustomerResponseDto dto = new () 
                { 
                    CustomerId = customerResponse.Customer.Id,
                    CustomerName = user.FirstName + " " + user.LastName, 
                    PriceAUD = AUDPrice,
                    PriceUSD = USDPrice,
                    Timespan = user.PlanTimeSpan.ToLower(),
                    Certificates = certificatesCount,
                    CustomerEmail = user.Email,
                    PlanName = planInfos[0].PlanName,
                    PlanId = planId,
                    AppId = _paymentService.SquareConfig.ApplicationId,
                    LocationId = _paymentService.SquareConfig.LocationId,
                    IsTemplate = user.IsTemplate
                };
                return Ok(dto);

            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { ErrorMessage = e.Message, InnerErrorMessage = e.InnerException?.Message, StackTrace = e.StackTrace });
            }
        }


        #region free trial manage


        [HttpPost("free-trial-checkuser")]
        public IActionResult FreeTialValidateUser([FromForm] string email)
        {
            try
            {
                bool user =  _userService.FindMainAppUserByEmail(email);
                return Ok(user);
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { ErrorMessage = e.Message, InnerErrorMessage = e.InnerException?.Message, StackTrace = e.StackTrace });
            }
        }


        [HttpPost("free-trial-signup")]
        public async Task<IActionResult> FreeTrialEmail([FromForm]TrialRegister trialRegister)
        {
            try
            {
                bool user = _userService.FindMainAppUserByEmail(trialRegister.Email);
                if (user)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = StatusCodes.Status400BadRequest, Content = ErrorMessages.UserExists, IsSuccess = false });
                }
                if (!Utils.ValidatePassword(trialRegister.Password))
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = StatusCodes.Status400BadRequest, Content = ErrorMessages.PasswordValidationFailed, IsSuccess = false });
                }

                string hash = Guid.NewGuid().ToString();
                string encryptedPassword = Encryption.EncryptPassword(trialRegister.Password);
                hash += encryptedPassword;
                int id = await _userService.CreateTempRegister(trialRegister.Email, hash, encryptedPassword);
                string encodedHash = System.Web.HttpUtility.UrlEncode(hash);
                SignUpMail signUpMail = new()
                {
                    Email = trialRegister.Email,
                    Subject = "Verify your account - VeriDoc Certificate",
                    VerifyLink = _configuration["WebsiteURL"] + $"account/verify?email={trialRegister.Email}&hash={encodedHash}"
                };
                _mailService.SendFreeSignUpMail(signUpMail);
                return Ok(new App.Entity.Models.HttpResponse() { Content = "Mail sent", IsSuccess = true, StatusCode = StatusCodes.Status200OK });
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { ErrorMessage = e.Message, InnerErrorMessage = e.InnerException?.Message, StackTrace = e.StackTrace });
            }
        }



        [HttpPost("verify")]
        public async Task<IActionResult> VerifyEmailSignup([FromForm]string email, [FromForm]string hash)
        {
            try
            {
                RegisterTempMail? registerTemp = await _userService.FindTempRegister(email);
                if (registerTemp == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { Content = ErrorMessages.UserInvalid, StatusCode = StatusCodes.Status400BadRequest, IsSuccess = false });
                }
                if (!registerTemp.Hash.Equals(hash))
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { Content = ErrorMessages.UserInvalid, StatusCode = StatusCodes.Status400BadRequest, IsSuccess = false });
                }
                if (registerTemp.IsCompleted)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { Content = ErrorMessages.UserExists, StatusCode = StatusCodes.Status400BadRequest, IsSuccess = false });
                }

                List<PlanInfo> planInfos = await _paymentService.GetPlanInfo("free");
                if (planInfos.Count == 0)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { Content = ErrorMessages.ServerHasNoPlan, StatusCode = StatusCodes.Status400BadRequest, IsSuccess = false });
                }
                List<OrganisationPlan> organisationPlans = _userService.GetOrganisationPlans();
                OrganisationPlan? organisationPlan = organisationPlans.FirstOrDefault(e => "free" == e.plan_name.ToLower());
                if (organisationPlan == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { Content = ErrorMessages.ServerHasNoPlan, StatusCode = StatusCodes.Status400BadRequest, IsSuccess = false });
                }

                CreateCustomerResponse? customerResponse = await _paymentService.CreateCsutomer(new RegisterUser() { Email = email });
                if (customerResponse == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { Content = ErrorMessages.UserCreationFail, StatusCode = StatusCodes.Status400BadRequest, IsSuccess = false });
                }

                OrganisationDeatailsDto deatailsDto = new()
                {
                    OrganisationName = StorageHelper.GetOrganisationName(email),
                    PlanId = organisationPlan.plan_id,
                    AdminPassword = registerTemp.Password,
                    ContactEmail = registerTemp.Email,
                    SubscriptionCounter = planInfos[0].Certificates,
                    OrganisationShortName = StorageHelper.CreateOrganisationBucketName(StorageHelper.GetOrganisationName(email))[..3].ToUpper(),
                    SquareCustomerId = customerResponse.Customer.Id,
                    IsBillingSame = true
                };

                int creationSuccessfull = await _userService.AddOrganisation(deatailsDto);
                if (creationSuccessfull == 1)
                {
                    RegisterUser registerUser = new()
                    {
                        Email = email,
                        IsActive = true,
                        Plan = planInfos[0].PlanName,
                        PlanInfoId = planInfos[0].Id,
                        PlanTimeSpan = planInfos[0].Timespan,
                        CustomerId = customerResponse.Customer.Id,
                        PaymentStatus = _paymentService.PaymentStatusSuccess
                    };
                    await _userService.CreateAsync(registerUser);

                    AppUser appUser = new()
                    {
                        Email = email,
                        Password = registerTemp.Password,
                        IsActive = true,
                        CustomerId = customerResponse.Customer.Id,
                        PlanTimeSpan = planInfos[0].Timespan,
                        PlanInfoId = planInfos[0].Id,
                        Plan = planInfos[0].PlanName,
                        PaymentStatus = _paymentService.PaymentStatusSuccess
                    };
                    int id = await _userService.CreateAppUserAsync(appUser);

                    App.Entity.Models.Plan.Subscription subscription = new()
                    {
                        AppUserId = id,
                        StartTime = DateTime.UtcNow,
                        EndTime = DateTime.UtcNow.AddMonths(1),
                        CustomerId = customerResponse.Customer.Id,
                        Timespan = appUser.PlanTimeSpan,
                        Certificates = planInfos[0].Certificates,
                        PlanInfoId = appUser.PlanInfoId,
                        Status = _paymentService.SubscriptionStatusSuccess,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _userService.CreateSubscriptionAsync(subscription);

                    SubscriptionHistory subscriptionHistory = new()
                    {
                        AppUserId = id,
                        CustomerId = customerResponse.Customer.Id,
                        PlanName = planInfos[0].PlanName,
                        TimeSpan = planInfos[0].Timespan,
                        Certificates = planInfos[0].Certificates,
                        Price = 0,
                        SubscriptionId = "",
                        StartTime = subscription.StartTime,
                        EndTime = subscription.EndTime,
                        Status = "ACTIVE"
                    };

                    await _paymentService.CreateSubscriptionHistory(subscriptionHistory);

                    registerTemp.IsCompleted = true;
                    await _userService.UpdateTempRegister(registerTemp);
                    return StatusCode(StatusCodes.Status201Created, new App.Entity.Models.HttpResponse() { Content = registerTemp.Password, IsSuccess = true, StatusCode = StatusCodes.Status201Created });
                }

                return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, IsSuccess = false, Content = ErrorMessages.UserExists });
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { ErrorMessage = e.Message, InnerErrorMessage = e.InnerException?.Message, StackTrace = e.StackTrace });
            }
        }


        private static RegisterUser CreateUser(SignupDto signupDto, List<CountryModel> countryModels, List<OrganisationPlan> organisationPlans)
        {
            int? countryId = null,billingCountryId = null;
            if (!string.IsNullOrEmpty(signupDto.CountryCode))
            {
                string countryCode = signupDto.CountryCode.Replace("+", "").Trim();
                CountryModel? country = countryModels.FirstOrDefault(e => e.PHONECODE == int.Parse(countryCode));
                countryId = country?.COUNTRY_ID;
            }
            if (!string.IsNullOrEmpty(signupDto.BillingCountryCode))
            {
                string billingcountryCode = signupDto.BillingCountryCode.Replace("+", "").Trim();
                CountryModel? billingCountry = countryModels.FirstOrDefault(e => e.PHONECODE == int.Parse(billingcountryCode));
                billingCountryId = billingCountry?.COUNTRY_ID;
            }
            
            OrganisationPlan? organisationPlan = organisationPlans.FirstOrDefault(e => e.plan_name.ToLower() == signupDto.Plan.ToLower() && e.Cadence.ToLower() == signupDto.PlanTimeSpan.ToLower());

            return new RegisterUser()
            {
                FirstName = signupDto.FirstName,
                LastName = signupDto.LastName,
                Email = signupDto.Email,
                PhoneCode = signupDto.PhoneCode,
                PhoneNumber = signupDto.PhoneNumber,
                CompanyName = signupDto.CompanyName,
                Address = signupDto.Address,
                City = signupDto.City,
                State = signupDto.State,
                Zip = signupDto.Zip,
                Country = signupDto.Country,
                CountryCode = signupDto.CountryCode,
                CountryId = countryId,
                BillingCompanyName = signupDto.BillingCompanyName,
                BillingAddress = signupDto.BillingAddress,
                BillingCity = signupDto.BillingCity,
                BillingState = signupDto.BillingState,
                BillingZip = signupDto.BillingZip,
                BillingCountry = signupDto.BillingCountry,
                BillingCountryCode = signupDto.BillingCountryCode,
                BillingCountryId = billingCountryId,
                Plan = signupDto.Plan,
                PlanTimeSpan = signupDto.PlanTimeSpan.ToLower() == "yearly" ? "Yearly" : "Monthly",
                MainAppPlanId = organisationPlan!.plan_id,
                IsBillingSame = signupDto.IsBiilingSame,
                IsTemplate = signupDto.IsTemplate
            };
        }


        #endregion


        #region free trial plan upgrade

        [HttpPost("validate-customer")]
        public async Task<IActionResult> ValidateCustomerId([FromBody]PlanDetailsDto plan)
        {
            try
            {
                AppUser? appUser = await _userService.FindAppUserByCustomerId(plan.CustomerId);
                if (appUser == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.InvalidCustomerID });
                }
                App.Entity.Models.Plan.Subscription subscription = await _paymentService.GetSubscription(plan.CustomerId);
                if (subscription == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.InvalidCustomerID });
                }
                if (!string.IsNullOrEmpty(subscription.SubscriptionId))
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.InvalidCustomerID });
                }
                List<PlanInfo> planInfos = await _paymentService.GetPlanInfo(plan.PlanName);
                List<OrganisationPlan> organisationPlans = _userService.GetOrganisationPlans();
                if (planInfos.Count == 0)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.PlanError });
                }
                if (!planInfos.Any(e => e.Timespan.ToLower() == plan.PlanTimespan.ToLower()))
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.PlanTimeError });
                }
                if (!organisationPlans.Any(e => e.Cadence.ToLower() == plan.PlanTimespan.ToLower()))
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.PlanTimeError });
                }
                if (!organisationPlans.Any(e => e.plan_name.ToLower() == plan.PlanName.ToLower()))
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.PlanTimeError });
                }
                double aud = await CurrencyConvertor.GetAUD();
                string? customerName = appUser.FirstName + " " + appUser.LastName;
                customerName = string.IsNullOrEmpty(customerName) ? appUser.Email : customerName;
                decimal USDPrice = plan.PlanTimespan.ToLower() == "yearly" ? planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "yearly")!.Price * 12 : planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "monthly")!.Price;
                decimal AUDPrice = USDPrice * Convert.ToDecimal(aud);
                int certificatesCount = plan.PlanTimespan.ToLower() == "yearly" ? planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "yearly")!.Certificates : planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "monthly")!.Certificates;
                string? planId = _paymentService.GetSubscriptionPlanId(planInfos, plan.PlanTimespan);

                CustomerResponseDto dto = new() 
                { 
                    CustomerId = plan.CustomerId,
                    CustomerName = customerName,
                    PriceAUD = AUDPrice,
                    PriceUSD = USDPrice, 
                    Timespan = plan.PlanTimespan.ToLower(),
                    Certificates = certificatesCount,
                    CustomerEmail = appUser.Email!, 
                    PlanName = planInfos[0].PlanName,
                    PlanId = planId,
                    AppId = _paymentService.SquareConfig.ApplicationId,
                    LocationId = _paymentService.SquareConfig.LocationId 
                };
                return Ok(dto);
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { ErrorMessage = e.Message, InnerErrorMessage = e.InnerException?.Message, StackTrace = e.StackTrace });
            }
        }


        [HttpPost("update-subscription")]
        public async Task<IActionResult> UpdateSubscription([FromBody] SubscriptionDto responseDto)
        {
            try
            {
                if (string.IsNullOrEmpty(responseDto.PlanTimespan)) { return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.PlanTimeError }); }
                List<PlanInfo> planInfos = await _paymentService.GetPlanInfo(responseDto.PlanName);
                List<OrganisationPlan> organisationPlans = _userService.GetOrganisationPlans();
                if (planInfos.Count == 0)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.PlanError });
                }
                if (!planInfos.Any(e => e.Timespan.ToLower() == responseDto.PlanTimespan.ToLower()))
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.PlanTimeError });
                }
                if (!organisationPlans.Any(e => e.Cadence.ToLower() == responseDto.PlanTimespan.ToLower()))
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.PlanTimeError });
                }
                if (!organisationPlans.Any(e => e.plan_name.ToLower() == responseDto.PlanName.ToLower()))
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.PlanTimeError });
                }
                AppUser? appUser = await _userService.FindAppUserByEmailAsync(responseDto.CustomerEmail);
                if (appUser == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.UserInvalid });
                }
                RegisterUser registerUser = (await _userService.FindByEmailAsync(responseDto.CustomerEmail))!;
                App.Entity.Models.Plan.Subscription subscription = await _paymentService.GetSubscription(responseDto.CustomerId);

                CreateCardResponse? cardResponse = await _paymentService.CreateCard(responseDto.CustomerId, responseDto.CardToken, responseDto.HolderName, responseDto.Address, responseDto.State, responseDto.Zip);
                if (cardResponse == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.InvalidCardData });
                }
                subscription.CardId = cardResponse.Card.Id;
                subscription.PlanInfoId = planInfos.FirstOrDefault(e => e.PlanName.ToLower() == responseDto.PlanName.ToLower() && e.Timespan.ToLower() == responseDto.PlanTimespan.ToLower())!.Id;
                subscription.Timespan = responseDto.PlanTimespan;
                await _paymentService.UpdateSubscription(subscription);

                CreateSubscriptionResponse? subscriptionResponse = await _paymentService.CreateSubscription(responseDto.PlanId, responseDto.CustomerId, cardResponse.Card.Id);
                if (subscriptionResponse == null)
                {
                    return BadRequest(new App.Entity.Models.HttpResponse() { StatusCode = 400, Content = ErrorMessages.SubscriptionError });
                }
                
                registerUser.CustomerId = responseDto.CustomerId;
                registerUser.CardId = cardResponse.Card.Id;
                registerUser.CardToken = responseDto.CardToken;
                registerUser.SquareSubscriptionId = subscriptionResponse.Subscription.Id;
                registerUser.Plan = responseDto.PlanName;
                registerUser.PlanTimeSpan = responseDto.PlanTimespan;
                registerUser.PlanId = responseDto.PlanId;
                registerUser.PaymentStatus = _paymentService.PaymentStatusSuccess;
                registerUser.IsActive = true;
                registerUser.PlanInfoId = planInfos.FirstOrDefault(e => e.PlanName.ToLower() == responseDto.PlanName.ToLower() && e.Timespan.ToLower() == responseDto.PlanTimespan.ToLower())!.Id;
                registerUser.MainAppPlanId = organisationPlans.FirstOrDefault(e => e.plan_name.ToLower() == responseDto.PlanName.ToLower() && e.Cadence.ToLower() == responseDto.PlanTimespan.ToLower())!.plan_id;
                await _userService.UpdateAsync(registerUser);

                appUser.CustomerId = responseDto.CustomerId;
                appUser.CardId = cardResponse.Card.Id;
                appUser.CardToken = responseDto.CardToken;
                appUser.SquareSubscriptionId = subscriptionResponse.Subscription.Id;
                appUser.Plan = responseDto.PlanName;
                appUser.PlanTimeSpan = responseDto.PlanTimespan;
                appUser.PlanId = responseDto.PlanId;
                appUser.PaymentStatus = _paymentService.PaymentStatusSuccess;
                appUser.IsActive = true;
                appUser.PlanInfoId = planInfos.FirstOrDefault(e => e.PlanName.ToLower() == responseDto.PlanName.ToLower() && e.Timespan.ToLower() == responseDto.PlanTimespan.ToLower())!.Id;
                await _userService.UpdateAppUserAsync(appUser);

                _userService.UpdateOrganisationPlan(responseDto.CustomerId, registerUser.MainAppPlanId);

                return Ok(new
                {
                    planName = responseDto.PlanName,
                    planPricing = responseDto.PlanTimespan.ToLower() == "yearly" ? planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "yearly")?.Price : planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "monthly")?.Price,
                    planDescription = responseDto.PlanTimespan.ToLower() == "yearly" ? planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "yearly")?.Description : planInfos.FirstOrDefault(e => e.Timespan.ToLower() == "monthly")?.Description,
                    startDate = subscriptionResponse.Subscription.StartDate,
                    endDate = subscriptionResponse.Subscription.ChargedThroughDate
                });
            }
            catch (Exception e)
            {
                LoggerHelper.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorModel() { ErrorMessage = e.Message, InnerErrorMessage = e.InnerException?.Message, StackTrace = e.StackTrace });
            }
        }

        #endregion
    }
}
