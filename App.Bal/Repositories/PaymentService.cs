using App.Bal.Services;
using App.Dal;
using App.Entity.Config;
using App.Entity.Models;
using App.Entity.Models.Plan;
using App.Entity.Models.SignUp;
using App.Foundation.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Square;
using Square.Exceptions;
using Square.Models;
using Square.Utilities;

namespace App.Bal.Repositories
{
    public class PaymentService : IPaymentService
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly SquareConfig _squareConfig;
        private readonly SquareClient _squareClient;
        private readonly AppDbContext _appDbConttext;

        public SquareConfig SquareConfig => _squareConfig;

        #region Square Payment Management

        public PaymentService(Microsoft.Extensions.Configuration.IConfiguration configuration, AppDbContext appDbContext)
        {
            _configuration = configuration;
            _squareConfig = new SquareConfig();
            _configuration.GetSection(SquareConfig.Path).Bind(_squareConfig);
            Square.Environment environment = _squareConfig.SquareEnvironment == "sandbox" ? Square.Environment.Sandbox : Square.Environment.Production;
            _squareClient = new SquareClient.Builder().
                Environment(environment).
                AccessToken(_squareConfig.AccessToken).
                SquareVersion(_squareConfig.Version).
                Build();
            _appDbConttext = appDbContext;
        }


        public async Task<CreateCardResponse?> CreateCard(string CustomerId, string Token, string? holderName, string? address, string? state, string? zip)
        {
            Address address1 = new Address.Builder()
                .AddressLine1(address)
                .AddressLine2(state)
                //.PostalCode(zip)
                .Build();

            var carddata = new Card.Builder()
              .CustomerId(CustomerId)
              .CardholderName(holderName)
              .BillingAddress(address1)
              .Build();

            var body = new CreateCardRequest.Builder(
                idempotencyKey: NewIdempotencyKey(),
                sourceId: Token,
                card: carddata)
              .Build();

            try
            {
                return await _squareClient.CardsApi.CreateCardAsync(body: body);
            }
            catch (ApiException e)
            {
                if (e.Errors != null) 
                {
                    
                    string message = string.Join(",", e.Errors.Select(e => e.Detail).ToList()) + System.Environment.NewLine + e.StackTrace;
                    LoggerHelper.LogError(new Exception(message));
                }
                return null;
            }
        }


        public async Task<CreateCustomerResponse?> CreateCsutomer(RegisterUser appUser)
        {
            var uuid = NewIdempotencyKey();

            var sqCompany = !string.IsNullOrEmpty(appUser.CompanyName) ? appUser.CompanyName : !string.IsNullOrEmpty(appUser.BillingCompanyName) ? appUser.BillingCompanyName : null;
            var sqAddress1 = !string.IsNullOrEmpty(appUser.Address) ? appUser.Address : !string.IsNullOrEmpty(appUser.BillingAddress) ? appUser.BillingAddress : null; ;
            var sqAddress2 = !string.IsNullOrEmpty(appUser.City) ? appUser.City + "," + appUser.State : !string.IsNullOrEmpty(appUser.BillingCity) ? appUser.BillingCity + ", " + appUser.BillingState : null; ;
            var sqAddressZip = !string.IsNullOrEmpty(appUser.Zip) ? appUser.Zip : !string.IsNullOrEmpty(appUser.BillingZip) ? appUser.BillingZip : null; ;
            var sqAddressCountry = !string.IsNullOrEmpty(appUser.Country) ? appUser.Country : !string.IsNullOrEmpty(appUser.BillingCountry) ? appUser.BillingCountry : null; ;

            var address = new Address.Builder()
                .AddressLine1(sqAddress1)
                .AddressLine2(sqAddress2)
                .PostalCode(sqAddressZip)
                .Country(sqAddressCountry)
                .FirstName(appUser.FirstName)
                .LastName(appUser.LastName)
                .Build();

            var body = new CreateCustomerRequest.Builder()
                     .IdempotencyKey(uuid)
                     .GivenName(appUser.FirstName)
                     .FamilyName(appUser.LastName)
                     .CompanyName(sqCompany)
                     .EmailAddress(appUser.Email)
                     .Address(address)
                     .Build();

            try
            {
                return await _squareClient.CustomersApi.CreateCustomerAsync(body);
            }
            catch (ApiException e)
            {
                if (e.Errors != null)
                {
                    string message = string.Join(",", e.Errors.Select(e => e.Detail).ToList()) + System.Environment.NewLine + e.StackTrace;
                    LoggerHelper.LogError(new Exception(message));
                }
                return null;
            }
        }

        public async Task<CreateSubscriptionResponse?> CreateSubscription(string planId, string customerId, string cardId)
        {
            var body = new CreateSubscriptionRequest.Builder(
                locationId: _squareConfig.LocationId,
                planId: planId,
                customerId: customerId)
                .IdempotencyKey(NewIdempotencyKey())
                .CardId(cardId)
                .Build();
            try
            {
                return await _squareClient.SubscriptionsApi.CreateSubscriptionAsync(body);
            }
            catch (ApiException e)
            {
                if (e.Errors != null)
                {
                    string message = string.Join(",", e.Errors.Select(e => e.Detail).ToList()) + System.Environment.NewLine + e.StackTrace;
                    LoggerHelper.LogError(new Exception(message));
                }
                return null;
            }
        }

        public async Task<RetrieveCustomerResponse?> RetrieveCustomer(string customerId)
        {
            try
            {
                return await _squareClient.CustomersApi.RetrieveCustomerAsync(customerId: customerId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<RetrieveSubscriptionResponse> RetrieveSubscription(string subscriptionId)
        {
            return await _squareClient.SubscriptionsApi.RetrieveSubscriptionAsync(subscriptionId: subscriptionId);
        }

        public async Task<RetrieveCardResponse> RetrieveCard(string cardId)
        {
            return await _squareClient.CardsApi.RetrieveCardAsync(cardId: cardId);
        }

        public async Task<CancelSubscriptionResponse> CancelSubscription(string subscriptionId)
        {
            try
            {
                return await _squareClient.SubscriptionsApi.CancelSubscriptionAsync(subscriptionId: subscriptionId);
            }
            catch (ApiException e)
            {
                LoggerHelper.LogError(e);
                return null;
            }
        }

        public async Task<GetInvoiceResponse> GetInvoice(string? invoiceId)
        {
            return await _squareClient.InvoicesApi.GetInvoiceAsync(invoiceId: invoiceId);
        }

        public async Task<RetrieveOrderResponse> RetrieveOrder(string? orderId)
        {
            return await _squareClient.OrdersApi.RetrieveOrderAsync(orderId: orderId);
        }

        public async Task<DisableCardResponse> DisableCard(string cardId)
        {
            try
            {
                return await _squareClient.CardsApi.DisableCardAsync(cardId: cardId);
            }
            catch (ApiException e)
            {
                return null;
            }
        }
        private static string NewIdempotencyKey()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<CreatePaymentResponse?> CreatePayment(decimal price, string customerId, string sourceId)
        {
            price *= 100;
            var amountMoney = new Money.Builder()
              .Amount((long)price)
              .Currency("AUD")
              .Build();

            var body = new CreatePaymentRequest.Builder(sourceId: sourceId, idempotencyKey: NewIdempotencyKey(), amountMoney)
              .CustomerId(customerId)
              .Build();

            try
            {
                return await _squareClient.PaymentsApi.CreatePaymentAsync(body: body);
            }
            catch (ApiException e)
            {
                if (e.Errors != null)
                {
                    string message = string.Join(',', e.Errors.Select(e => e.Detail).ToList()) + System.Environment.NewLine + e.StackTrace;
                    LoggerHelper.LogError(new Exception(message));
                }
                return null;
            }
        }


        public async Task<CreatePaymentResponse?> CreatePayment(decimal price, string customerId, string refId, string sourceId)
        {
            price *= 100;
            var amountMoney = new Money.Builder()
              .Amount((long)price)
              .Currency("AUD")
              .Build();

            var body = new CreatePaymentRequest.Builder(
                sourceId: sourceId, 
                idempotencyKey: NewIdempotencyKey(), 
                amountMoney)
                .ReferenceId(refId)
                .CustomerId(customerId)
                .Build();

            try
            {
                return await _squareClient.PaymentsApi.CreatePaymentAsync(body: body);
            }
            catch (ApiException e)
            {
                if (e.Errors != null)
                {
                    string message = string.Join(',', e.Errors.Select(e => e.Detail).ToList()) + System.Environment.NewLine + e.StackTrace;
                    LoggerHelper.LogError(new Exception(message));
                }
                return null;
            }
        }

        #endregion

        public Task<List<PlanInfo>> GetPlanInfo(string plan)
        {
            return _appDbConttext.PlanInfos.Where(e => e.PlanName.ToLower() == plan.ToLower()).ToListAsync();
        }

        public async Task<PlanInfo?> GetPlanInfo(int id)
        {
            return await _appDbConttext.PlanInfos.FindAsync(id);
        }

        public async Task<bool> ValidateRequest(HttpRequest httpRequest)
        {
            var signature = httpRequest.Headers["x-square-hmacsha256-signature"].ToString() ?? "";
            var req = httpRequest.Body;
            var requestBody = await new StreamReader(req).ReadToEndAsync();
            return WebhooksHelper.IsValidWebhookEventSignature(requestBody, signature, _squareConfig.SquareSignatureKey, _squareConfig.SquareNotificationUrl);
        }

        public async Task<int> CreateSubscriptionHistory(SubscriptionHistory history)
        {
            await _appDbConttext.SubscriptionHistories.AddAsync(history);
            await _appDbConttext.SaveChangesAsync();
            return history.Id;
        }

        public async Task<Entity.Models.Plan.Subscription> GetSubscription(string customerId)
        {
            return await _appDbConttext.Subscriptions.Include(e => e.PlanInfo).FirstOrDefaultAsync(e => e.CustomerId == customerId);
        }

        public async Task<int> UpdateSubscription(Entity.Models.Plan.Subscription subscription)
        {
            _appDbConttext.Subscriptions.Update(subscription);
            await _appDbConttext.SaveChangesAsync();
            return subscription.Id;
        }

        public async Task<List<SubscriptionHistory>> GetSubscriptionHistory(string customerId)
        {
            return await _appDbConttext.SubscriptionHistories.
                Where(e => e.CustomerId == customerId).
                GroupBy(e => e.SubscriptionId).
                Select(e => e.First()).
                ToListAsync();
        }

        public string GetSubscriptionPlanId(List<PlanInfo> planInfos, string timespan)
        {
            if (planInfos.FirstOrDefault()?.PlanName.ToLower() == "standard")
            {
                return timespan.ToLower().Equals("monthly") ? _squareConfig.StandardPlanIdMonthy : _squareConfig.StandardPlanIdAnnually;
            }
            else if (planInfos.FirstOrDefault()?.PlanName.ToLower() == "pro")
            {
                return timespan.ToLower().Equals("monthly") ? _squareConfig.ProPlanIdMonthly : _squareConfig.ProPlanIdAnnually;
            }
            else
            {
                return "";
            }
        }

        public async Task<List<PlanInfo>> GetPlans()
        {
            return await _appDbConttext.PlanInfos.ToListAsync();
        }

        public async Task<List<Entity.Models.Plan.Subscription>> GetSubscriptionsAsync(int batchSize, int index)
        {
            return await _appDbConttext.Subscriptions.
                Include(e => e.PlanInfo).
                Where(e => e.PlanInfo.Timespan.ToLower() == "yearly").
                Where(e => e.IsActive && e.Status == "ACTIVE").
                Where(e => e.EndTime > DateTime.UtcNow).
                Where(e => e.CurrentMonth < 12).
                Where(e => DateTime.UtcNow > e.StartTime.AddMonths(e.CurrentMonth)).
                Skip(batchSize * index).
                Take(batchSize).
                ToListAsync();
        }

        public async Task<int> GetSubscriptionsCountAsync()
        {
            return await _appDbConttext.Subscriptions.
                Include(e => e.PlanInfo).
                Where(e => e.PlanInfo.Timespan.ToLower() == "yearly").
                Where(e => e.IsActive && e.Status == "ACTIVE").
                Where(e => e.EndTime > DateTime.UtcNow).
                Where(e => e.CurrentMonth < 12).
                Where(e => DateTime.UtcNow > e.StartTime.AddMonths(e.CurrentMonth)).
                CountAsync();
        }

        public async Task<List<Entity.Models.Plan.Subscription>> GetSubscriptionsTrialReminderAsync(int batchSize, int index)
        {

            return await _appDbConttext.Subscriptions.
                Include(e => e.PlanInfo).
                Include(e => e.AppUser).
                Where(e => e.IsActive).
                Where(e => e.PlanInfo.PlanName.ToLower() == "free").
                OrderBy(E => E.Id).
                Take(batchSize).
                Skip(index * batchSize).
                ToListAsync();
        }


        public async Task<int> GetSubscriptionsTrialRemiderCountAsync()
        {
            return await _appDbConttext.Subscriptions.
                Include(e => e.PlanInfo).
                Where(e => e.IsActive).
                Where(e => e.PlanInfo.PlanName.ToLower() == "free").
                CountAsync();
        }


        public async Task<int> UpdateSubscriptionsAsync(List<Entity.Models.Plan.Subscription> subscriptions)
        {
            _appDbConttext.Subscriptions.UpdateRange(subscriptions);
            return await _appDbConttext.SaveChangesAsync();
        }

        public async Task<bool> CreateTransectionHistory(TransectionHistory transectionHistory)
        {
            if (transectionHistory is not null)
            {
                await _appDbConttext.TransectionHistories.AddAsync(transectionHistory);
                await _appDbConttext.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<bool> UpdateTransectionHistory(TransectionHistory transectionHistory)
        {
            if (transectionHistory is not null)
            {
                _appDbConttext.TransectionHistories.Update(transectionHistory);
                await _appDbConttext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<TransectionHistory>> GetTransectionHistory(string email)
        {
            return await _appDbConttext.TransectionHistories.Where(e => e.Email == email).ToListAsync();
        }

        public async Task<TransectionHistory?> GetTransectionHistoryByPaymentId(string? paymentId)
        {
            return await _appDbConttext.TransectionHistories.FirstOrDefaultAsync(e => e.PaymentId == paymentId);
        }


        //public async Task<List<Entity.Models.Plan.Subscription>> GetSubscriptionsTrialRemindeEndrAsync(int batchSize, int index)
        //{
        //    return await _appDbConttext.Subscriptions.
        //        Skip(index * batchSize).
        //        Take(batchSize).
        //        Include(e => e.PlanInfo).
        //        Include(e => e.AppUser).
        //        Where(e => e.IsActive).
        //        Where(e => e.PlanInfo.PlanName.ToLower() == "free").
        //        Where(e => e.EndTime <= DateTime.UtcNow).
        //        ToListAsync();
        //}

        //public async Task<int> GetSubscriptionsTrialRemiderEndCountAsync()
        //{
        //    return await _appDbConttext.Subscriptions.
        //        Include(e => e.PlanInfo).
        //        Include(e => e.AppUser).
        //        Where(e => e.IsActive).
        //        Where(e => e.PlanInfo.PlanName.ToLower() == "free").
        //        Where(e => e.EndTime <= DateTime.UtcNow).
        //        CountAsync();
        //}
    }
}
