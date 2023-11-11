using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models.Mail
{
    public class CustomPlan
    {
        [Required(ErrorMessage = "Name is Requried")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Business Name is Requried")]
        public string BusinessName { get; set; }

        [Required(ErrorMessage = "Email is Requried")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        public string Phone { get; set; }
        public string Certificates { get; set; }
        public string Request { get; set; }
        public string Duration { get; set; }
        public string Cadence { get; set; }
        public string OrganisationName { get; set; }
        public string OrganisationId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }

    }
}
