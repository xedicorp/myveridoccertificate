using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using App.Entity.Validation;

namespace App.Entity.Dto
{
    public class CustomerResponseDto
    {
        [Required(ErrorMessage = "Customer id is required")]
        public string CustomerId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Customer email is required")]
        public string CustomerEmail { get; set; } = string.Empty;
        public string PlanName { get; set; } = string.Empty;

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

        public string? CustomerName { get; set; }
        public string? Timespan { get; set; }
        public int Certificates { get; set; }
        public string? AppId { get; set; }
        public string? LocationId { get; set; }
        public bool IsTemplate { get; set; }
    }

}
