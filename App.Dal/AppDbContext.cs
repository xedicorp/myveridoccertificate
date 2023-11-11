using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App.Entity.Models.Plan;
using App.Entity.Models;
using App.Entity.Models.SignUp;
using App.Entity.Models.MainApp;

namespace App.Dal
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<PlanInfo> PlanInfos { get; set; }  
        public DbSet<AppUser> AppUsers { get; set; }  
        public DbSet<Subscription> Subscriptions { get; set; }  
        public DbSet<RegisterUser> RegisterUsers { get; set; }  
        public DbSet<SubscriptionHistory> SubscriptionHistories { get; set; }
        public DbSet<RegisterTempMail> RegisterTempMails { get; set; }
        public DbSet<TransectionHistory> TransectionHistories { get; set; }
    }
}
