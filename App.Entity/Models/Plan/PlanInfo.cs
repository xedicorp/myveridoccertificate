using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models.Plan
{
    public class PlanInfo
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        public string Timespan { get; set; }
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Certificates { get; set; }
        public decimal CertificatesPrice { get; set; }
        public decimal TemplatePrice { get; set; }
    }
}
