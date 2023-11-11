using System.ComponentModel.DataAnnotations;

namespace VeriDocCertificate.CofoundaryCMS;

public class FeatureContentDataModel : IPageBlockTypeDataModel
{
    [Required]
    [NestedDataModelCollection(IsOrderable = true, MinItems = 3, MaxItems = 6)]
    public ICollection<FeatureContentListDataModel> FeatureLists { get; set; }
}
