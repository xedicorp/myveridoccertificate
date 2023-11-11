﻿using Cofoundry.Domain.Data;

namespace Cofoundry.Domain.Internal;

public class PageRenderDetailsMapper : IPageRenderDetailsMapper
{
    private readonly IPageTemplateMicroSummaryMapper _pageTemplateMapper;
    private readonly IPageRenderSummaryMapper _pageRenderSummaryMapper;
    private readonly IOpenGraphDataMapper _openGraphDataMapper;

    public PageRenderDetailsMapper(
        IPageTemplateMicroSummaryMapper pageTemplateMapper,
        IOpenGraphDataMapper openGraphDataMapper,
        IPageRenderSummaryMapper pageRenderSummaryMapper
        )
    {
        _pageTemplateMapper = pageTemplateMapper;
        _openGraphDataMapper = openGraphDataMapper;
        _pageRenderSummaryMapper = pageRenderSummaryMapper;
    }

    /// <summary>
    /// Maps the main properties on a PageRenderDetails including
    /// page regions, but does not map the page block data.
    /// </summary>
    /// <param name="dbPageVersion">
    /// PageVersion record from the database. Must include the 
    /// OpenGraphImageAsset, PageTemplate and PageTemplate.PageTemplateRegions
    /// properties.
    /// </param>
    /// <param name="pageRoute">
    /// The page route to map to the new object.
    /// </param>
    public virtual PageRenderDetails Map(
        PageVersion dbPageVersion,
        PageRoute pageRoute
        )
    {
        ArgumentNullException.ThrowIfNull(dbPageVersion);
        ArgumentNullException.ThrowIfNull(pageRoute);

        var page = _pageRenderSummaryMapper.Map<PageRenderDetails>(dbPageVersion, pageRoute);

        MapInternal(dbPageVersion, page);

        return page;
    }

    /// <summary>
    /// Maps the main properties on a PageRenderDetails including
    /// page regions, but does not map the page block data.
    /// </summary>
    /// <param name="dbPageVersion">
    /// PageVersion record from the database. Must include the 
    /// OpenGraphImageAsset, PageTemplate and PageTemplate.PageTemplateRegions
    /// properties.
    /// </param>
    /// <param name="pageRouteLookup">
    /// Set of page routes to lookup the route property value.
    /// </param>
    public virtual PageRenderDetails Map(PageVersion dbPageVersion, IDictionary<int, PageRoute> pageRouteLookup)
    {
        ArgumentNullException.ThrowIfNull(dbPageVersion);
        ArgumentNullException.ThrowIfNull(pageRouteLookup);

        var page = _pageRenderSummaryMapper.Map<PageRenderDetails>(dbPageVersion, pageRouteLookup);

        MapInternal(dbPageVersion, page);

        return page;
    }

    protected void MapInternal(PageVersion dbPageVersion, PageRenderDetails page)
    {
        page.Template = _pageTemplateMapper.Map(dbPageVersion.PageTemplate);

        page.Regions = dbPageVersion
            .PageTemplate
            .PageTemplateRegions
            .Select(r => new PageRegionRenderDetails()
            {
                PageTemplateRegionId = r.PageTemplateRegionId,
                Name = r.Name
                // Blocks mapped elsewhere
            })
            .ToList();
    }
}
