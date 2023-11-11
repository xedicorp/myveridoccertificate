using Microsoft.AspNetCore.Html;

namespace Cofoundry.Web;

public class TryNowCTADisplayModel : IPageBlockTypeDisplayModel
{
    public string SectionName { get; set; }

    public string BackgroundColor { get; set; }
    public IHtmlContent Title { get; set; }
    public string LinkBtn { get; set; }


  
}