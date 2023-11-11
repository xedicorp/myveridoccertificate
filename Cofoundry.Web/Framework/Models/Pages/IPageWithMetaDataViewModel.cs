using Cofoundry.Domain.Domain.HeaderFooter.Models;
using Cofoundry.Domain.Domain.SeoTools.Models;

namespace Cofoundry.Web;

public interface IPageWithMetaDataViewModel
{
    string PageTitle { get; set; }
    string MetaDescription { get; set; }
    string MetaTitle { get; set; }
    string MetaKeywords { get; set; }

    HeaderFooterDetails HeaderFooterDetail { get; set; }
    SeoToolsDetails SeoToolsDetails { get; set; }
    public OpenGraphData OpenGraph { get; set; }
}
