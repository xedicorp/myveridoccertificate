using System.ComponentModel.DataAnnotations;

namespace App.Entity.Dto
{
    public class CardDetailDto
    {    
        [Required(ErrorMessage = "Customer id required")]
        public string CustomerId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Card token is required")]
        public string CardToken { get; set; } = string.Empty;

        public string HolderName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
    }
}
