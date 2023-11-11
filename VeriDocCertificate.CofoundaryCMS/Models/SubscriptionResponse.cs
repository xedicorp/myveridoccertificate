namespace VeriDocCertificate.CofoundaryCMS.Models
{
    public class SubscriptionResponse
    {
        public string CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string PlanName { get; set; }
        public decimal PriceUSD { get; set; }
        public decimal PriceAUD { get; set; }
        public string PlanId { get; set; }
        public string CardToken { get; set; }
        public string PlanTimespan { get; set; }
        public bool IsTemplate { get; set; }
        public string HolderName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
