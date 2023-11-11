namespace VeriDocCertificate.CofoundaryCMS;

/// <summary>
/// An ICustomEntityDetailsDisplayViewModel implementation is required if
/// you want to use a page template to dynamically render a details view
/// of a custom entity. This provides us with a strongly typed model to use
/// in the template.
/// </summary>
public class SocialMediaLinkDisplayModel : ICustomEntityPageDisplayModel<SocialMediaLinkDataModel>
{
    
    public string Title { get; set; }

    public string URL { get; set; }
    public string ClassName { get; set; }
    public string PageTitle { get; set ; }
    public string MetaDescription { get ; set; }
    public string MetaTitle { get ; set; }
    public string MetaKeywords { get; set; }
}