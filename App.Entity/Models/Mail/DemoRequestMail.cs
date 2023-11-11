using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models.Mail
{
    public class DemoRequestMail
    {
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(255)]
        public string FirstName { get; set; } = string.Empty;

 
        public string? LastName { get; set; } 

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        public string? PhoneCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Companyname { get; set; }


        [Required(ErrorMessage = "Coutry is required")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required")]
        public string Date { get; set; } = string.Empty;

        [Required(ErrorMessage = "Time is required")]
        public string Time { get; set; } = string.Empty;
    }
}
