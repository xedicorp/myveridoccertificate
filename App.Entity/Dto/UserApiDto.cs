using System.ComponentModel.DataAnnotations;

namespace App.Entity.Dto
{
    public class UserApiDto
    {
        [Required(ErrorMessage = "Customer Id is required")]
        public string CustomerId { get; set; } = string.Empty;
        public string Feedback { get; set; }
        public string OrganisationName { get; set; }
        public string OrganisationId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
    }
}
