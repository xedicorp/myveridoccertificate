using System.ComponentModel.DataAnnotations;

namespace VeriDocCertificate.CofoundaryCMS;

public class PricingDataModel : IPageBlockTypeDataModel
{
    [Required]
    [NestedDataModelCollection(IsOrderable = true, MinItems = 3, MaxItems = 6)]
    public ICollection<PricingListDataModel> Lists { get; set; }
}
