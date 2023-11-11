using App.Entity.Config;
using App.Entity.Models;
using App.Entity.Models.Plan;
using App.Entity.Models.SignUp;
using Microsoft.AspNetCore.Http;
using Square.Models;

namespace App.Bal.Services
{
    public interface IPaymentService
    {
        public string PaymentStatusFail { get { return "FAILED"; } }
        public string PaymentStatusSuccess { get { return "SUCCESS"; } }
        public string PaymentStatusPending { get { return "PENDING"; } }

        public string SubscriptionStatusFail { get { return "INACTIVE"; } }
        public string SubscriptionStatusSuccess { get { return "ACTIVE"; } }
        public string PaymentTypeSubscription { get { return "Subscription"; } }
        public string PaymentTypePayment { get { return "Payment"; } }

        #region Square

        public SquareConfig SquareConfig { get; }
        public Task<CreateCustomerResponse?> CreateCsutomer(RegisterUser appUser);
        public Task<CreateCardResponse?> CreateCard(string CustomerId, string Token, string? holderName, string? address, string? state, string? zip);
        public Task<RetrieveCardResponse> RetrieveCard(string cardId);
        public Task<DisableCardResponse> DisableCard(string cardId);
        public Task<CreateSubscriptionResponse?> CreateSubscription(string planId, string customerId, string cardId);

        public Task<RetrieveCustomerResponse?> RetrieveCustomer(string customerId);
        public Task<RetrieveSubscriptionResponse> RetrieveSubscription(string subscriptionId);
        
        public Task<CancelSubscriptionResponse> CancelSubscription(string subscriptionId);
        public Task<GetInvoiceResponse> GetInvoice(string? invoiceId);
        public Task<RetrieveOrderResponse> RetrieveOrder(string? orderId);

        public Task<CreatePaymentResponse?> CreatePayment(decimal price, string customerId, string sourceId);
        public Task<CreatePaymentResponse?> CreatePayment(decimal price, string customerId, string refId, string sourceId);
        #endregion

        public Task<Entity.Models.Plan.Subscription> GetSubscription(string customerId);
        public Task<List<PlanInfo>> GetPlanInfo(string plan);
        public Task<PlanInfo?> GetPlanInfo(int id);
        public Task<List<PlanInfo>> GetPlans();
        public Task<bool> ValidateRequest(HttpRequest httpRequest);

        #region Subscription management
        public Task<bool> CreateTransectionHistory(TransectionHistory transectionHistory);
        public Task<bool> UpdateTransectionHistory(TransectionHistory transectionHistory);
        public Task<List<TransectionHistory>> GetTransectionHistory(string email);
        public Task<TransectionHistory?> GetTransectionHistoryByPaymentId(string? paymentId);

        public Task<List<Entity.Models.Plan.Subscription>> GetSubscriptionsAsync(int batchSize, int index);
        public Task<List<Entity.Models.Plan.Subscription>> GetSubscriptionsTrialReminderAsync(int batchSize, int index);

        public Task<int> GetSubscriptionsCountAsync();
        public Task<int> GetSubscriptionsTrialRemiderCountAsync();


        public Task<int> CreateSubscriptionHistory(SubscriptionHistory history);
        public Task<List<SubscriptionHistory>> GetSubscriptionHistory(string customerId);
        public Task<int> UpdateSubscription(Entity.Models.Plan.Subscription subscription);
        public Task<int> UpdateSubscriptionsAsync(List<Entity.Models.Plan.Subscription> subscriptions);
        public string GetSubscriptionPlanId(List<PlanInfo> planInfos, string timespan);

        #endregion
    }
}
