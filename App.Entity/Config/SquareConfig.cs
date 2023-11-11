using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entity.Config
{
    public class SquareConfig
    {
        public const string Path = "Square";

        public string SquareEnvironment { get; set; } = string.Empty;   
        public string AccessToken { get; set; } = string.Empty;  
        public string AccessTokenSandbox { get; set; } = string.Empty;  
        public string LocationId { get; set; } = string.Empty;   
        public string Version { get; set; } = string.Empty;   
        public string ApplicationId { get; set; } = string.Empty;   
        public string ApplicationIdSandbox { get; set; } = string.Empty;   
        public string ProPlanIdMonthly { get; set; } = string.Empty;   
        public string ProPlanIdMonthlySandbox { get; set; } = string.Empty;   
        public string ProPlanIdAnnually { get; set; } = string.Empty;   
        public string ProPlanIdAnnuallySandbox { get; set; } = string.Empty;   
        public string StandardPlanIdMonthy { get; set; } = string.Empty;   
        public string StandardPlanIdMonthySandbox { get; set; } = string.Empty;   
        public string StandardPlanIdAnnually { get; set; } = string.Empty;   
        public string StandardPlanIdAnnuallySandbox { get; set; } = string.Empty;   
        public string SquareSignatureKey { get; set; } = string.Empty;   
        public string SquareNotificationUrl { get; set; } = string.Empty;   

    }
}
