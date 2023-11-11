using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Dto
{
    public class SignupDto
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = string.Empty;
        public string? PhoneCode { get; set; }
        public string? PhoneNumber { get; set; }

        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? CountryCode { get; set; }
        public string? Zip { get; set; }
        public bool IsBiilingSame { get; set; }
        public string? BillingCompanyName { get; set; }
        public string? BillingAddress { get; set; }
        public string? BillingCity { get; set; }
        public string? BillingState { get; set; }
        public string? BillingCountry { get; set; }
        public string? BillingCountryCode { get; set; }
        public string? BillingZip { get; set; }

        [Required(ErrorMessage = "Plan is required")]
        public string Plan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Plan TimeSpan is required")]
        public string PlanTimeSpan { get; set; } = string.Empty;
        public bool IsTemplate { get; set; }
    }
}
