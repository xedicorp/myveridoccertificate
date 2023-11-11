using Cofoundry.Domain.Data;
using Cofoundry.Domain.Domain.HeaderFooter.Models;
using Cofoundry.Domain.Domain.SeoTools.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Cofoundry.Domain.Internal;

/// <summary>
/// Gets a projection of a page that contains the data required to render a page, including template 
/// data for all the content-editable regions.
/// </summary>
public class GetPageRenderDetailsByIdQueryHandler
    : IQueryHandler<GetPageRenderDetailsByIdQuery, PageRenderDetails>
    , IPermissionRestrictedQueryHandler<GetPageRenderDetailsByIdQuery, PageRenderDetails>
{
    private readonly CofoundryDbContext _dbContext;
    private readonly IQueryExecutor _queryExecutor;
    private readonly IPageRenderDetailsMapper _pageRenderDetailsMapper;
    private readonly IEntityVersionPageBlockMapper _entityVersionPageBlockMapper;
    private readonly IImageAssetRouteLibrary _imageAssetRouteLibrary;
    private readonly IDocumentAssetRouteLibrary _documentAssetRouteLibrary;
    public GetPageRenderDetailsByIdQueryHandler(
        CofoundryDbContext dbContext,
        IQueryExecutor queryExecutor,
        IPageRenderDetailsMapper pageMapper,
        IEntityVersionPageBlockMapper entityVersionPageBlockMapper,
          IImageAssetRouteLibrary imageAssetRouteLibrary,
             IDocumentAssetRouteLibrary documentAssetRouteLibrary
        )
    {
        _dbContext = dbContext;
        _queryExecutor = queryExecutor;
        _pageRenderDetailsMapper = pageMapper;
        _entityVersionPageBlockMapper = entityVersionPageBlockMapper;
        _imageAssetRouteLibrary = imageAssetRouteLibrary;
        _documentAssetRouteLibrary = documentAssetRouteLibrary;
    }

    public async Task<PageRenderDetails> ExecuteAsync(GetPageRenderDetailsByIdQuery query, IExecutionContext executionContext)
    {
        var dbPage = await QueryPageAsync(query, executionContext);
        if (dbPage == null) return null;

        var pageRouteQuery = new GetPageRouteByIdQuery(dbPage.PageId);
        var pageRoute = await _queryExecutor.ExecuteAsync(pageRouteQuery, executionContext);

        var page = _pageRenderDetailsMapper.Map(dbPage, pageRoute);

        var dbPageBlocks = await QueryPageBlocks(page).ToListAsync();
        var allBlockTypes = await _queryExecutor.ExecuteAsync(new GetAllPageBlockTypeSummariesQuery(), executionContext);
        
        page.HeaderFooterDetail = await GetHeaderFooterSetting();

        page.SeoToolsDetails = await GetSeoToolsDetails();

        await _entityVersionPageBlockMapper.MapRegionsAsync(dbPageBlocks, page.Regions, allBlockTypes, query.PublishStatus, executionContext);

        return page;
    }
    private async Task<SeoToolsDetails> GetSeoToolsDetails()
    {
        var dbQuery = _dbContext
         .CustomEntityPublishStatusQueries
          .Include(v => v.CustomEntityVersion)
         .Include(v => v.CustomEntity)
         .AsNoTracking()
         .FilterByCustomEntityDefinitionCode("XSCHMA")
         .Where(p => p.PublishStatusQueryId == 1);

        var menuItems = dbQuery.ToList();
        var menuLinks = new List<SchemaItem>();
        foreach (var menuItem in menuItems)
        {
            CustomSchemaItem customHeaderMenuItem = JsonConvert.DeserializeObject<CustomSchemaItem>(menuItem.CustomEntityVersion.SerializedData);
            menuLinks.Add(new SchemaItem
            {
                Id = customHeaderMenuItem.Id,
                Name = customHeaderMenuItem.Name,
                CustomEntityId = menuItem.CustomEntityId

            });
        }

        SeoToolsDetails details = new SeoToolsDetails();
        var SeoTool = await _dbContext.SeoTools.FirstOrDefaultAsync();
        if (SeoTool != null)
        {
            details.ViewPort = SeoTool.ViewPort;
            details.CopyRight = SeoTool.CopyRight;
            details.Author = SeoTool.Author;
            details.ReplyTo = SeoTool.ReplyTo;
            details.Robots = SeoTool.Robots;
            details.ContentLanguage = SeoTool.ContentLanguage;
            details.Audience = SeoTool.Audience;
            details.RevisitAfter = SeoTool.RevisitAfter;
            details.Distribution = SeoTool.Distribution;
            details.AltDistribution = SeoTool.AltDistribution;
            details.Publisher = SeoTool.Publisher;
            details.AltCopyRight = SeoTool.AltCopyRight;
            details.Rel = SeoTool.Rel;
            details.Href = SeoTool.Href;
            details.HrefLang = SeoTool.HrefLang;
            details.GoogleTagManager = SeoTool.GoogleTagManager;
            details.GoogleAnalytics = SeoTool.GoogleAnalytics;
            details.GoogleSiteVerification = SeoTool.GoogleSiteVerification;
            details.BingSiteVerification = SeoTool.BingSiteVerification;
            details.SchemaIds = SeoTool.SchemaIds;
            details.SchemaItems = GetSchemaItems(SeoTool.SchemaIds, menuLinks);

        }
        return details;

    }
    private List<CustomSchemaItem> GetSchemaItems(string ids, List<SchemaItem> allLinks)
    {
        List<CustomSchemaItem> items = new List<CustomSchemaItem>();
        if (!string.IsNullOrEmpty(ids))
        {
            var arrIds = ids.Split(new char[] { ',' }).ToList();
            var q = from p in allLinks
                    where arrIds.Contains(p.CustomEntityId.ToString())
                    select new CustomSchemaItem
                    {
                        Id = p.Id,
                        Name = p.Name
                    };
            if (q != null)
            {
                items.AddRange(q);
            }
        }
        return items;
    }
    private async Task<HeaderFooterDetails> GetHeaderFooterSetting()
    {
        var dbQuery = _dbContext
           .CustomEntityPublishStatusQueries
            .Include(v => v.CustomEntityVersion)
           .Include(v => v.CustomEntity)
           .AsNoTracking()
           .FilterByCustomEntityDefinitionCode("HEMENU")
           .Where(p => p.PublishStatusQueryId == 1);

        var menuItems = dbQuery.ToList();
        var menuLinks = new List<HeaderMenuItem>();
        foreach (var menuItem in menuItems)
        {
            CustomHeaderMenuItem customHeaderMenuItem = JsonConvert.DeserializeObject<CustomHeaderMenuItem>(menuItem.CustomEntityVersion.SerializedData);
            menuLinks.Add(new HeaderMenuItem
            {
                Text = menuItem.CustomEntityVersion.Title,
                Url = customHeaderMenuItem.Url,
                CustomEntityId = menuItem.CustomEntityId,
                ClassName = customHeaderMenuItem.ClassName

            });
        }

        //Social Media Icons
        var dbQueryMedia = _dbContext
          .CustomEntityPublishStatusQueries
           .Include(v => v.CustomEntityVersion)
          .Include(v => v.CustomEntity)
          .AsNoTracking()
          .FilterByCustomEntityDefinitionCode("XMEDIA")
          .Where(p => p.PublishStatusQueryId == 1);

        var menuItemsMedia = dbQueryMedia.ToList();
        var menuLinksMedia = new List<HeaderMenuItem>();
        foreach (var menuItem in menuItemsMedia)
        {
            CustomHeaderMenuItem customHeaderMenuItem = JsonConvert.DeserializeObject<CustomHeaderMenuItem>(menuItem.CustomEntityVersion.SerializedData);
            menuLinksMedia.Add(new HeaderMenuItem
            {
                Text = menuItem.CustomEntityVersion.Title,
                Url = customHeaderMenuItem.Url,
                CustomEntityId = menuItem.CustomEntityId,
                ClassName = customHeaderMenuItem.ClassName

            });
        }


        var result = await _dbContext.HeaderFooterSettings.FirstOrDefaultAsync();
        return new HeaderFooterDetails
        {
            Address = result?.Address,
            Phone = result?.Phone,
            Email = result?.Email,
            HeaderLogoImageId = result?.HeaderLogoImageId,
            FooterLogoImageId = result?.FooterLogoImageId,
            PartnerLogoImageId = result?.PartnerLogoImageId,
            HeaderMenuItems = GetLinks(result?.HeaderMenuLinks, menuLinks),
            UsefulLinks = GetLinks(result?.UsefulMenuLinks, menuLinks),
            SocialMediaIcons = GetSocialMeidiaLinks(result?.SocialMediaLinks, menuLinksMedia),
            HeaderLogoImageUrl=await GetImageUrl(result?.HeaderLogoImageId ?? 0),
             FooterLogoImageUrl = await GetImageUrl(result?.FooterLogoImageId ?? 0),
             PartnerLogoImageUrl = await GetImageUrl(result?.PartnerLogoImageId ?? 0)
        };

    }

    //private async Task<string> GetImageUrl(int customEntityId)
    //{
    //     return   await _imageAssetRouteLibrary.ImageAssetAsync(customEntityId) ;
       
        
    //}
    private async Task<string> GetImageUrl(int customEntityId)
    {
        return await _documentAssetRouteLibrary.DocumentAssetAsync(customEntityId); 
    }
    private List<HeaderMenuItem> GetSocialMeidiaLinks(string ids, List<HeaderMenuItem> allLinks)
    {
        List<HeaderMenuItem> items = new List<HeaderMenuItem>();
        if (!string.IsNullOrEmpty(ids))
        {
            var arrIds = ids.Split(new char[] { ',' }).ToList();
            var q = from p in allLinks
                    where arrIds.Contains(p.CustomEntityId.ToString())
                    select p;
            if (q != null)
            {
                items.AddRange(q);
            }
        }
        return items;
    }
    private List<HeaderMenuItem> GetLinks(string ids, List<HeaderMenuItem> allLinks)
    {
        List<HeaderMenuItem> items = new List<HeaderMenuItem>();
        if (!string.IsNullOrEmpty(ids))
        {
            //var arrIds = ids.Split(new char[] { ',' }).ToList();
            //var q = from p in allLinks
            //        where arrIds.Contains(p.CustomEntityId.ToString())
            //        select p;
            //if (q != null)
            //{
            //    items.AddRange(q);
            //}
            var ints = ids.Split(",").Select(i => Int32.Parse(i)).ToList();
            foreach (var intId in ints)
            {

                var item = allLinks.FirstOrDefault(p => p.CustomEntityId == intId);
                if (item != null)
                {
                    items.Add(item);
                }
            }
        }
        return items;
    }
    private async Task<PageVersion> QueryPageAsync(GetPageRenderDetailsByIdQuery query, IExecutionContext executionContext)
    {
        PageVersion result;

        if (query.PublishStatus == PublishStatusQuery.SpecificVersion)
        {
            if (!query.PageVersionId.HasValue)
            {
                throw new Exception("A PageVersionId must be included in the query to use PublishStatusQuery.SpecificVersion");
            }

            result = await _dbContext
                .PageVersions
                .AsNoTracking()
                .Include(v => v.Page)
                .Include(v => v.OpenGraphImageAsset)
                .Include(v => v.PageTemplate)
                .ThenInclude(t => t.PageTemplateRegions)
                .FilterActive()
                .FilterByPageId(query.PageId)
                .FilterByPageVersionId(query.PageVersionId.Value)
                .FirstOrDefaultAsync();
        }
        else
        {
            var queryResult = await _dbContext
                .PagePublishStatusQueries
                .AsNoTracking()
                .Include(q => q.PageVersion)
                .ThenInclude(v => v.Page)
                .Include(q => q.PageVersion)
                .ThenInclude(v => v.OpenGraphImageAsset)
                .Include(q => q.PageVersion)
                .ThenInclude(v => v.PageTemplate)
                .ThenInclude(t => t.PageTemplateRegions)
                .FilterActive()
                .FilterByStatus(query.PublishStatus, executionContext.ExecutionDate)
                .FilterByPageId(query.PageId)
                .FirstOrDefaultAsync();

            result = queryResult?.PageVersion;
        }

        return result;
    }

    private IQueryable<PageVersionBlock> QueryPageBlocks(PageRenderDetails page)
    {
        return _dbContext
            .PageVersionBlocks
            .FilterActive()
            .AsNoTracking()
            .Where(m => m.PageVersionId == page.PageVersionId);
    }

    public IEnumerable<IPermissionApplication> GetPermissions(GetPageRenderDetailsByIdQuery query)
    {
        yield return new PageReadPermission();
    }
}
