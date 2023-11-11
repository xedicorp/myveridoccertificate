namespace VeriDocCertificate.CofoundaryCMS;

public class FeatureContentDisplayModel : IPageBlockTypeDisplayModel
{
    public ICollection<FeatureContentListDisplayModel> FeatureLists { get; set; }
}
