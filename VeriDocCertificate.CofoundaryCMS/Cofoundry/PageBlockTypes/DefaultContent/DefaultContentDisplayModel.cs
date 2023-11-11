using Microsoft.AspNetCore.Html;

namespace Cofoundry.Web;

public class DefaultContentDisplayModel : IPageBlockTypeDisplayModel
{
    public string SectionName { get; set; }

    public string BackgroundColor { get; set; }

    public string SectionTitle { get; set; }

    public string SectionSubTitle { get; set; }

    public string Orientation { get; set; }

    public string SectionImageID { get; set; }

    public IHtmlContent SectionDescription { get; set; }
  
}