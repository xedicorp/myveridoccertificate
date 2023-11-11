using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Dto
{
    public class PaymentDto
    {
        [Required]
        public string SubscriptionId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public bool PaymentStatus { get; set; }
    }
}
