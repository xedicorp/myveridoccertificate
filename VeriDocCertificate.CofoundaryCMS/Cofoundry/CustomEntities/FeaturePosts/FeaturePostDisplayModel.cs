namespace VeriDocCertificate.CofoundaryCMS;

/// <summary>
/// An ICustomEntityDetailsDisplayViewModel implementation is required if
/// you want to use a page template to dynamically render a details view
/// of a custom entity. This provides us with a strongly typed model to use
/// in the template.
/// </summary>
public class FeaturePostDisplayModel : ICustomEntityPageDisplayModel<FeaturePostDataModel>
{
    public string PageTitle { get; set; }


    public string MetaDescription { get; set; }
    public string MetaTitle { get; set; }
    public string MetaKeywords { get; set; }
    public string Title { get; set; }

    public DateTime Date { get; set; }

    public string FullPath { get; set; }

    public ICollection<FeatureCategorySummary> FeatureCategories { get; set; }
}