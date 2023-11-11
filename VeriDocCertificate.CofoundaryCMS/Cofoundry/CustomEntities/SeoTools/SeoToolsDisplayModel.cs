using System.ComponentModel.DataAnnotations;

namespace VeriDocCertificate.CofoundaryCMS;

/// <summary>
/// An ICustomEntityDetailsDisplayViewModel implementation is required if
/// you want to use a page template to dynamically render a details view
/// of a custom entity. This provides us with a strongly typed model to use
/// in the template.
/// </summary>
public class SeoToolsDisplayModel : ICustomEntityDisplayModel<SeoToolsDataModel>
{
    

    public string GoogleAnalytics { get; set; }
    public string BingVerification { get; set; }

    public string GoogleVerification { get; set; }
    
    public string GoogleTagManager { get; set; }
}