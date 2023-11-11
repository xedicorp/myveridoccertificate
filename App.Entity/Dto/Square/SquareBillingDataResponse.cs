using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entity.Dto.Square
{
    public class SquareBillingDataResponse
    {
        public string? billingDate { get; set; }
        public string? squareInvoiceLink { get; set; }
        public string? servicePeriod { get; set; }
        public string? planDescription { get; set; }
        public string cardType { get; set; } = string.Empty;
        public string? cardDigits { get; set; }
        public string? amount { get; set; }
    }
}
