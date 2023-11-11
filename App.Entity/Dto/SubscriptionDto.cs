using App.Entity.Validation;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Dto
{
    public class SubscriptionDto
    {
        [Required(ErrorMessage = "Customer id is required")]
        public string CustomerId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Customer email is required")]
        public string CustomerEmail { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;
        public string? PlanTimespan { get; set; }

        [Required(ErrorMessage = "Price USD is required")]
        [PriceValidation]
        public decimal PriceUSD { get; set; }

        [Required(ErrorMessage = "Price AUD is required")]
        [PriceValidation]
        public decimal PriceAUD { get; set; }

        [Required(ErrorMessage = "Plan id is required")]
        public string PlanId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Card token is required")]
        public string CardToken { get; set; } = string.Empty;

        public string HolderName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
    }
}
