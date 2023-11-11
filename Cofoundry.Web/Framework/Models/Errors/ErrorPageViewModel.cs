using Cofoundry.Domain.Domain.HeaderFooter.Models;
using Cofoundry.Domain.Domain.SeoTools.Models;

namespace Cofoundry.Web;

public class ErrorPageViewModel : IErrorPageViewModel
{
    public int StatusCode { get; set; }

    public string StatusCodeDescription { get; set; }

    public string PageTitle { get; set; }

    public string MetaDescription { get; set; }

    public string PathBase { get; set; }

    public string Path { get; set; }

    public string QueryString { get; set; }
    public string MetaTitle { get; set; }
    public string MetaKeywords { get; set; }
    public HeaderFooterDetails HeaderFooterDetail { get;  set  ; }
    public SeoToolsDetails SeoToolsDetails { get ; set ; }
    public OpenGraphData OpenGraph { get; set ; }
}
