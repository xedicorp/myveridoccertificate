﻿using Cofoundry.Domain.Domain.HeaderFooter.Models;
using Cofoundry.Domain.Domain.SeoTools.Models;

namespace Cofoundry.Domain;

/// <summary>
/// Page data required to render a page, including template data for all the content-editable
/// regions. This object is specific to a particular version which may not always be the 
/// latest (depending on the query).
/// </summary>
public class PageRenderDetails : PageRenderSummary
{
    /// <summary>
    /// Page data required to render a page, including template data for all the content-editable
    /// regions. This object is specific to a particular version which may not always be the 
    /// latest (depending on the query).
    /// </summary>
    public PageRenderDetails()
        : base()
    {
    }

    /// <summary>
    /// The template used to render this page.
    /// </summary>
    public PageTemplateMicroSummary Template { get; set; }

    /// <summary>
    /// Content-editable page region and block data for rendering out to the template.
    /// </summary>
    public ICollection<PageRegionRenderDetails> Regions { get; set; }

    public HeaderFooterDetails HeaderFooterDetail { get; set; }
    public SeoToolsDetails SeoToolsDetails { get; set; }
}
