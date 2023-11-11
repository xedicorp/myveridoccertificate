using System.ComponentModel.DataAnnotations;
using VeriDocCertificate.CofoundaryCMS.Models.Enum;

namespace VeriDocCertificate.CofoundaryCMS;

public class PricingListDataModel : INestedDataModel
{
    
    //Plan Title
    [PreviewTitle]
    [Required]
    [Display(Description = "Title to display in the slide.")]
    [MaxLength(10)]
    public string Title { get; set; }

    //plan MonthlyDesctiption
    [Display(Description = "Formatted text to display in the slide.")]
    [Required]
    [Html(HtmlToolbarPreset.Headings, HtmlToolbarPreset.Custom, CustomToolbar = "undo redo | bold italic underline | numlist | link unlink | code ")]
    public string MonthlyPlanDescription { get; set; }

    //plan YearlyDesctiption
    [Display(Description = "Formatted text to display in the slide.")]

    [Html(HtmlToolbarPreset.Headings, HtmlToolbarPreset.Custom, CustomToolbar = "undo redo | bold italic underline | numlist | link unlink | code ")]
    public string YearlyPlanDescription { get; set; }


    //plan Features
    [Display(Description = "Formatted text to display in the slide.")]
    [Required]
    [Html(HtmlToolbarPreset.Headings, HtmlToolbarPreset.Custom, CustomToolbar = "undo redo | bold italic underline | numlist | link unlink | code ")]
    public string PlanFeatures { get; set; }

    //Plan Monthly Link
    [SelectList(typeof(isFreetrial))]
    public isFreetrial FreeTrialLink { get; set; }
    //Plan Monthly Link

    public string PlanMonthlyLink { get; set; }
    //Plan yearly Link

    public string PlanYearlyLink { get; set; }


}
