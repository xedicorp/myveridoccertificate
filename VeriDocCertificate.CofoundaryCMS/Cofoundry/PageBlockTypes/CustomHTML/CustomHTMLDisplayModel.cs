using Microsoft.AspNetCore.Html;

namespace Cofoundry.Web;

public class CustomHTMLDisplayModel : IPageBlockTypeDisplayModel
{
    public string SectionName { get; set; } 

    public IHtmlContent SectionHTML { get; set; }
  
}