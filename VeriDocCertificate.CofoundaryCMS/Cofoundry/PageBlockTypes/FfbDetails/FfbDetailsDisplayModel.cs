using Microsoft.AspNetCore.Html;

namespace Cofoundry.Web;

public class FfbDetailsDisplayModel : IPageBlockTypeDisplayModel
{

    public string FfbTitle { get; set; }
    public string MeidaLookup { get; set; }

    public string VideoLookup { get; set; }

    public string FfbImageID { get; set; }

    public string FfbIframe { get; set; }

    public IHtmlContent FfbDescription { get; set; }
  
}