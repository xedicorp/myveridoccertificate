using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entity.Dto.Square
{
    public class PlanInfoResponseDto
    {
        public string? PlanName { get; set; } = string.Empty;
        public decimal Pricing { get; set; }
        public string? Description { get; set; }
        public string Cadence { get; set; } = string.Empty;
        public string? CardType { get; set; }
        public string? CardDigit { get; set; }
        public string ExpiryDate { get; set; } = string.Empty;
        public string RegisterDate { get; set; } = string.Empty;
        public string NextBill { get; set; } = string.Empty;
        public string BillPeriod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Certificate { get; set; }
    }
}
