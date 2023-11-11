using Cofoundry.Domain.Domain.HeaderFooter.Models;
using Cofoundry.Domain.Domain.SeoTools.Models;

namespace Cofoundry.Web;

public class PageViewModel : IPageViewModel
{
    public PageRenderDetails Page { get; set; }

    public bool IsPageEditMode { get; set; }

    public string PageTitle
    {
        get
        {
            if (Page == null) return null;
            return Page.Title;
        }
        set
        {
            SetPagePropertyNullCheck(nameof(PageTitle));
            Page.Title = value;
        }
    }

    public string MetaDescription
    {
        get
        {
            if (Page == null) return null;
            return Page.MetaDescription;
        }
        set
        {
            SetPagePropertyNullCheck(nameof(MetaDescription));
            Page.MetaDescription = value;
        }
    }
    public string MetaTitle
    {
        get
        {
            if (Page == null) return null;
            return Page.MetaTitle;
        }
        set
        {
            SetPagePropertyNullCheck(nameof(MetaTitle));
            Page.MetaTitle = value;
        }
    }
    public string MetaKeywords
    {
        get
        {
            if (Page == null) return null;
            return Page.MetaKeywords;
        }
        set
        {
            SetPagePropertyNullCheck(nameof(MetaKeywords));
            Page.MetaKeywords = value;
        }
    }

    public HeaderFooterDetails HeaderFooterDetail
    {
        get
        {
            if (Page == null) return null;
            return Page.HeaderFooterDetail;
        }
        set
        {
            SetPagePropertyNullCheck(nameof(HeaderFooterDetail));
            Page.HeaderFooterDetail = value;
        }
    }
    public SeoToolsDetails SeoToolsDetails
    {
        get
        {
            if (Page == null) return null;
            return Page.SeoToolsDetails;
        }
        set
        {
            SetPagePropertyNullCheck(nameof(SeoToolsDetails));
            Page.SeoToolsDetails = value;
        }
    }
    private void SetPagePropertyNullCheck(string property)
    {
        if (Page == null)
        {
            throw new NullReferenceException($"Cannot set the {property} property, the Page property has not been set.");
        }
    }

    public PageRoutingHelper PageRoutingHelper { get; set; }
    public OpenGraphData OpenGraph { get  ; set ; }
}
