using Cofoundry.Domain.Domain.HeaderFooter.Models;
using Cofoundry.Domain.Domain.SeoTools.Models;

namespace Cofoundry.Web;

/// <summary>
/// Simple implementation of IPageWithMetaDataViewModel for pages that don't need much else.
/// </summary>
public class SimplePageViewModel : IPageWithMetaDataViewModel
{
    public string PageTitle { get; set; }

    public string MetaDescription { get; set; }
    public string MetaTitle { get; set; }
    public string MetaKeywords { get; set; }
    public HeaderFooterDetails HeaderFooterDetail { get; set; }
    public SeoToolsDetails SeoToolsDetails { get ; set ; }
    public OpenGraphData OpenGraph { get ; set ; }
}
