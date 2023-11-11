﻿using Cofoundry.Domain.Data;

namespace Cofoundry.Domain.Internal;

public class PageRenderSummaryMapper : IPageRenderSummaryMapper
{
    private readonly IPageTemplateMicroSummaryMapper _pageTemplateMapper;
    private readonly IOpenGraphDataMapper _openGraphDataMapper;

    public PageRenderSummaryMapper(
        IPageTemplateMicroSummaryMapper pageTemplateMapper,
        IOpenGraphDataMapper openGraphDataMapper
        )
    {
        _pageTemplateMapper = pageTemplateMapper;
        _openGraphDataMapper = openGraphDataMapper;
    }

    /// <summary>
    /// Genric mapper for objects that inherit from PageRenderSummary.
    /// </summary>
    /// <param name="dbPageVersion">PageVersion record from the database. Must include the OpenGraphImageAsset property.</param>
    /// <param name="pageRoute">The page route to map to the new object.</param>
    public virtual T Map<T>(PageVersion dbPageVersion, PageRoute pageRoute)
        where T : PageRenderSummary, new()
    {
        ArgumentNullException.ThrowIfNull(dbPageVersion);
        ArgumentNullException.ThrowIfNull(pageRoute);

        var page = MapInternal<T>(dbPageVersion);
        page.PageRoute = pageRoute;

        return page;
    }

    /// <summary>
    /// Genric mapper for objects that inherit from PageRenderSummary.
    /// </summary>
    /// <param name="dbPageVersion">PageVersion record from the database. Must include the OpenGraphImageAsset property.</param>
    /// <param name="pageRouteLookup">Dictionary containing all page routes.</param>
    public virtual T Map<T>(PageVersion dbPageVersion, IDictionary<int, PageRoute> pageRouteLookup)
        where T : PageRenderSummary, new()
    {
        ArgumentNullException.ThrowIfNull(dbPageVersion);
        ArgumentNullException.ThrowIfNull(pageRouteLookup);

        var page = MapInternal<T>(dbPageVersion);

        page.PageRoute = pageRouteLookup.GetOrDefault(page.PageId);

        if (page.PageRoute == null)
        {
            throw new Exception($"Unable to locate a page route when mapping a {nameof(PageRenderSummary)} with an id of {page.PageId}.");
        }

        return page;
    }

    protected T MapInternal<T>(PageVersion dbPageVersion) where T : PageRenderSummary, new()
    {
        //ImageAssetRenderDetails img = new ImageAssetRenderDetails();
        //img.
        var page = new T()
        {
            MetaDescription = dbPageVersion.MetaDescription,
            PageId = dbPageVersion.PageId,
            PageVersionId = dbPageVersion.PageVersionId,
            Title = dbPageVersion.Title,
            MetaTitle = dbPageVersion.MetaTitle,
            MetaKeywords = dbPageVersion.MetaKeywords,
            WorkFlowStatus = (WorkFlowStatus)dbPageVersion.WorkFlowStatusId,
            CreateDate = dbPageVersion.CreateDate,
            OpenGraph=  _openGraphDataMapper.Map(dbPageVersion)
            //OpenGraph = new OpenGraphData { Title= dbPageVersion.OpenGraphTitle,
            //Description = dbPageVersion.OpenGraphDescription,
            // Image = img}

        };

        page.OpenGraph = _openGraphDataMapper.Map(dbPageVersion );
        return page;
    }
}
