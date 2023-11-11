using Microsoft.AspNetCore.Html;

namespace Cofoundry.Web;
public class FeatureContentListDisplayModel
{
    public DocumentAssetRenderDetails FeatureImageID { get; set; }
    public string FeatureTitle { get; set; }
    public IHtmlContent FeatureDescription { get; set; }
    public string FeatureImageUrl { get; set; }


}
