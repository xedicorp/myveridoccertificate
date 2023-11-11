using VeriDocCertificate.CofoundaryCMS.Models.Enum;

namespace VeriDocCertificate.CofoundaryCMS;

public class PricingListDisplayModel
{
    public string Title { get; set; }
    public string MonthlyPlanDescription { get; set; }
    public string YearlyPlanDescription { get; set; }

    public string PlanFeatures { get; set; }
    public isFreetrial FreeTrialLink { get; set; }
    public string PlanMonthlyLink { get; set; }

    public string PlanYearlyLink { get; set; }
   

   
}
