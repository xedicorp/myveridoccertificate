namespace VeriDocCertificate.CofoundaryCMS.Models
{
    public class SignupResponseModel
    {
        public string CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string PlanName { get; set; }
        public decimal PriceUSD { get; set; }
        public decimal PriceAUD { get; set; }
        public string PlanId { get; set; }
        public string CardToken { get; set; }
        public string CustomerName { get; set; }
        public string Timespan { get; set; }
        public int Certificates { get; set; }
        public string AppId { get; set; }
        public string LocationId { get; set; }
        public bool IsTemplate { get; set; }
    }
}
