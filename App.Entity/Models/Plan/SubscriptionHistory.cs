using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Entity.Models;

namespace App.Entity.Models.Plan
{
    public class SubscriptionHistory
    {
        [Key]
        public int Id { get; set; }

        public string CustomerId { get; set; }
        public string SubscriptionId { get; set; }
        public int AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser? AppUser { get; set; }

        public string PlanName { get; set; }
        public string TimeSpan { get; set; }
        public decimal Price { get; set; }  

        [Column(TypeName = "datetime2")]
        public DateTime StartTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EndTime { get; set; }
        public string? Status { get; set; }
        public int Certificates { get; set; }

    }
}
