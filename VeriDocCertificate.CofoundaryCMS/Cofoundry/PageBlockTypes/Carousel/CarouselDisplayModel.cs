namespace VeriDocCertificate.CofoundaryCMS;

public class CarouselDisplayModel : IPageBlockTypeDisplayModel
{
    public ICollection<CarouselListDisplayModel> Lists { get; set; }
}
