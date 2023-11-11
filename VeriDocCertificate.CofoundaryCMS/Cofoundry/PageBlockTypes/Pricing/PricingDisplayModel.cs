namespace VeriDocCertificate.CofoundaryCMS;

public class PricingDisplayModel : IPageBlockTypeDisplayModel
{
    public ICollection<PricingListDisplayModel> Lists { get; set; }
}
