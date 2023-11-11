using Microsoft.AspNetCore.Html;

namespace Cofoundry.Web;

public class FreeTrialBannerDisplayModel : IPageBlockTypeDisplayModel
{
    public string BannerImageID { get; set; }

    public IHtmlContent BannerDescription { get; set; }
  
}