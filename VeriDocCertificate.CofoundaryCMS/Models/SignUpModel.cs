namespace VeriDocCertificate.CofoundaryCMS.Models
{
    public class SignUpModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Zip { get; set; }
        public bool IsBiilingSame { get; set; }

        public string BillingCompanyName { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingCountry { get; set; }
        public string BillingCountryCode { get; set; }
        public string BillingZip { get; set; }

        public string Plan { get; set; }
        public string PlanTimeSpan { get; set; }
        public bool IsTemplate { get; set; }

    }
}
