using App.Foundation.Common;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models.Plan
{
    public class CreatePaymentModel
    {
        [Required(ErrorMessage = ErrorMessages.InvalidCustomerID)]
        public string CustomerId { get; set; } = string.Empty;

        [Required(ErrorMessage = ErrorMessages.InvalidPrice)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = ErrorMessages.InvalidCertificateQty)]
        public int Certificates { get; set; }
    }
}
