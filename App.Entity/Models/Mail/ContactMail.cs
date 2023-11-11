using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models.Mail
{
    public class ContactMail
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

        [StringLength(255)]
        public string Message { get; set; } = string.Empty;

        [Required(ErrorMessage = "Recaptcha Name is required")]
        public string RecaptchaToken { get; set; } = string.Empty;

    }
}
