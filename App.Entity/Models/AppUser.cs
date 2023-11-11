using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entity.Models.Plan;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace App.Entity.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class AppUser
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string? FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        public string? LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string? Email { get; set; } = string.Empty;
        public string? PhoneCode { get; set; }
        public string? PhoneNumber { get; set; }

        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Zip { get; set; }

        public string? BillingCompanyName { get; set; }
        public string? BillingAddress { get; set; }
        public string? BillingCity { get; set; }
        public string? BillingState { get; set; }
        public string? BillingCountry { get; set; }
        public string? BillingZip { get; set; }

        public string? CustomerId { get; set; }
        public string? CardToken { get; set; }
        public string? CardId { get; set; }
        public string? SquareSubscriptionId { get; set; }

        [Required(ErrorMessage = "Plan is required")]
        public string? Plan { get; set; }

        [Required(ErrorMessage = "Plan TimeSpan is required")]
        public string? PlanTimeSpan { get; set; }
        public string? PlanId { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentFailedReason { get; set; }
        public bool IsActive { get; set; }
        public string? Password { get; set; }
        public int PlanInfoId { get; set; }
        public bool IsTemplate { get; set; }
    }
}
