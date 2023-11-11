using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace App.Entity.Models.MainApp
{
    public class OrganisationPlan
    {
        [Key]
        public int plan_id { get; set; }
        public string plan_name { get; set; } = string.Empty;
        public string Cadence { get; set; } = string.Empty;
        public string? SquarePackageId { get; set; }
        public string? Description { get; set; }
        public int Certificates { get; set; }
        public decimal Price { get; set; }
    }
}
