using System.ComponentModel.DataAnnotations;

namespace VeriDocCertificate.CofoundaryCMS;

public class CarouselDataModel : IPageBlockTypeDataModel
{
    [Required]
    [NestedDataModelCollection(IsOrderable = true, MinItems = 2)]
    public ICollection<CarouselListDataModel> Lists { get; set; }
}
