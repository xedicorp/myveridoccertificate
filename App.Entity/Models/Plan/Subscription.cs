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
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        public int AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public virtual AppUser? AppUser { get; set; }
        public string CustomerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Timespan { get; set; }
        public int Certificates { get; set; }
        public string? SubscriptionId { get; set; }
        public string? CardId { get; set; }
        public string? Status { get; set; }
        public bool IsActive { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }

        public int PlanInfoId { get; set; }

        [ForeignKey("PlanInfoId")]
        public PlanInfo? PlanInfo { get; set; }

        public int CurrentMonth { get; set; }
    }
}
