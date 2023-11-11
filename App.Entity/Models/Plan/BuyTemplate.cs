using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models.Plan
{
    public class BuyTemplate
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string CustomerId { get; set; } = string.Empty;

        [Required]
        public int Template { get; set; }
    }
}
