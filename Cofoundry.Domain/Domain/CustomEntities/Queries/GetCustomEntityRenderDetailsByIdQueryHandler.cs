using Cofoundry.Domain.Data;
using Cofoundry.Domain.Domain.HeaderFooter.Models;
using Cofoundry.Domain.Domain.SeoTools.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Cofoundry.Domain.Internal;

/// <summary>
/// Query to retreive a custom entity by it's database id, projected as a
/// CustomEntityRenderDetails, which contains all data for rendering a specific 
/// version of a custom entity out to a page, including template data for all the 
/// content-editable page regions. This projection is specific to a particular 
/// version which may not always be the latest (depending on the query), and to a 
/// specific page. Although often you may only have one custom entity page, it is 
/// possible to have multiple.
/// </summary>
public class GetCustomEntityRenderDetailsByIdQueryHandler
    : IQueryHandler<GetCustomEntityRenderDetailsByIdQuery, CustomEntityRenderDetails>
    , IIgnorePermissionCheckHandler
{
    private readonly CofoundryDbContext _dbContext;
    private readonly ICustomEntityDataModelMapper _customEntityDataModelMapper;
    private readonly IEntityVersionPageBlockMapper _entityVersionPageBlockMapper;
    private readonly IPermissionValidationService _permissionValidationService;
    private readonly IQueryExecutor _queryExecutor;
    private readonly IImageAssetRouteLibrary _imageAssetRouteLibrary;
    private readonly IDocumentAssetRouteLibrary _documentAssetRouteLibrary;
    public GetCustomEntityRenderDetailsByIdQueryHandler(
        CofoundryDbContext dbContext,
        ICustomEntityDataModelMapper customEntityDataModelMapper,
        IEntityVersionPageBlockMapper entityVersionPageBlockMapper,
        IPermissionValidationService permissionValidationService,
        IQueryExecutor queryExecutor,
         IImageAssetRouteLibrary imageAssetRouteLibrary,
          IDocumentAssetRouteLibrary documentAssetRouteLibrary
        )
    {
        _dbContext = dbContext;
        _customEntityDataModelMapper = customEntityDataModelMapper;
        _entityVersionPageBlockMapper = entityVersionPageBlockMapper;
        _permissionValidationService = permissionValidationService;
        _queryExecutor = queryExecutor;
        _imageAssetRouteLibrary = imageAssetRouteLibrary;
        _documentAssetRouteLibrary = documentAssetRouteLibrary;
    }

    public async Task<CustomEntityRenderDetails> ExecuteAsync(GetCustomEntityRenderDetailsByIdQuery query, IExecutionContext executionContext)
    {
        var dbResult = await QueryCustomEntityAsync(query, executionContext);
        if (dbResult == null) return null;

        var entity = MapCustomEntity(dbResult, executionContext);

        if (dbResult.CustomEntity.LocaleId.HasValue)
        {
            var getLocaleQuery = new GetActiveLocaleByIdQuery(dbResult.CustomEntity.LocaleId.Value);
            entity.Locale = await _queryExecutor.ExecuteAsync(getLocaleQuery, executionContext);
        }

        var pageRoutesQuery = new GetPageRoutingInfoByCustomEntityIdQuery(dbResult.CustomEntityId);
        var pageRoutes = await _queryExecutor.ExecuteAsync(pageRoutesQuery, executionContext);
        entity.PageUrls = MapPageRoutings(pageRoutes, dbResult);

        var selectedRoute = pageRoutes.FirstOrDefault(r => r.PageRoute.PageId == query.PageId);

        if (selectedRoute != null)
        {
            var pageVersion = selectedRoute.PageRoute.Versions.GetVersionRouting(PublishStatusQuery.PreferPublished);
            if (pageVersion == null)
            {
                throw new Exception($"Error mapping routes: {nameof(pageVersion)} cannot be null. A page route should always have at least one version.");
            }

            entity.Regions = await GetRegionsAsync(pageVersion.PageTemplateId);
            var dbPageBlocks = await GetPageBlocksAsync(entity.CustomEntityVersionId, selectedRoute.PageRoute.PageId);

            var allBlockTypes = await _queryExecutor.ExecuteAsync(new GetAllPageBlockTypeSummariesQuery(), executionContext);
            await _entityVersionPageBlockMapper.MapRegionsAsync(dbPageBlocks, entity.Regions, allBlockTypes, query.PublishStatus, executionContext);
        }
        else
        {
            entity.Regions = Array.Empty<CustomEntityPageRegionRenderDetails>();
        }
        var cpageVersion = selectedRoute.PageRoute.Versions.GetVersionRouting(PublishStatusQuery.PreferPublished);
         var pageData = await _dbContext
              .PageVersions
              .AsNoTracking()
              .Include(v => v.Page)
              .Include(v => v.OpenGraphImageAsset)
              .Include(v => v.PageTemplate)
              .ThenInclude(t => t.PageTemplateRegions)
              .FilterActive()
              .FilterByPageId(query.PageId)
              .FilterByPageVersionId(cpageVersion.VersionId)
              .FirstOrDefaultAsync();
        entity.MetaTitle = pageData.MetaTitle;
        entity.MetaKeywords = pageData.MetaKeywords;
        entity.MetaDescription = pageData.MetaDescription;
        entity.HeaderFooterDetail = await GetHeaderFooterSetting();
        entity.SeoToolsDetails = await GetSeoToolsDetails();
        return entity;
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
            Address = result.Address,
            Phone = result.Phone,
            Email = result.Email,
            HeaderLogoImageId = result.HeaderLogoImageId,
            FooterLogoImageId = result.FooterLogoImageId,
            PartnerLogoImageId = result.PartnerLogoImageId,
            HeaderMenuItems = GetLinks(result.HeaderMenuLinks, menuLinks),
            UsefulLinks = GetLinks(result.UsefulMenuLinks, menuLinks),
            SocialMediaIcons = GetSocialMeidiaLinks(result.SocialMediaLinks, menuLinksMedia),
            HeaderLogoImageUrl = await GetImageUrl(result.HeaderLogoImageId.GetValueOrDefault()),
            FooterLogoImageUrl = await GetImageUrl(result.FooterLogoImageId.GetValueOrDefault()),
            PartnerLogoImageUrl = await GetImageUrl(result.PartnerLogoImageId.GetValueOrDefault())
        };

    }
    //private async Task<string> GetImageUrl(int customEntityId)
    //{
    //    return await _imageAssetRouteLibrary.ImageAssetAsync(customEntityId);


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

    private Task<List<CustomEntityVersionPageBlock>> GetPageBlocksAsync(int customEntityVersionId, int pageId)
    {
        return _dbContext
            .CustomEntityVersionPageBlocks
            .AsNoTracking()
            .FilterActive()
            .Where(m => m.CustomEntityVersionId == customEntityVersionId && m.PageId == pageId)
            .ToListAsync();
    }

    private Task<List<CustomEntityPageRegionRenderDetails>> GetRegionsAsync(int pageTemplateId)
    {
        return _dbContext
            .PageTemplateRegions
            .AsNoTracking()
            .Where(s => s.PageTemplateId == pageTemplateId)
            .Select(s => new CustomEntityPageRegionRenderDetails()
            {
                PageTemplateRegionId = s.PageTemplateRegionId,
                Name = s.Name
            })
            .ToListAsync();
    }

    private CustomEntityRenderDetails MapCustomEntity(CustomEntityVersion dbResult, IExecutionContext executionContext)
    {
        _permissionValidationService.EnforceCustomEntityPermission<CustomEntityReadPermission>(dbResult.CustomEntity.CustomEntityDefinitionCode, executionContext.UserContext);

        var entity = new CustomEntityRenderDetails()
        {
            CreateDate = dbResult.CreateDate,
            CustomEntityDefinitionCode = dbResult.CustomEntity.CustomEntityDefinitionCode,
            CustomEntityId = dbResult.CustomEntityId,
            CustomEntityVersionId = dbResult.CustomEntityVersionId,
            Ordering = dbResult.CustomEntity.Ordering,
            Title = dbResult.Title,
            UrlSlug = dbResult.CustomEntity.UrlSlug,
            WorkFlowStatus = (WorkFlowStatus)dbResult.WorkFlowStatusId,
            PublishDate = dbResult.CustomEntity.PublishDate
        };

        entity.PublishStatus = PublishStatusMapper.FromCode(dbResult.CustomEntity.PublishStatusCode);
        entity.Model = _customEntityDataModelMapper.Map(dbResult.CustomEntity.CustomEntityDefinitionCode, dbResult.SerializedData);

        return entity;
    }

    private async Task<CustomEntityVersion> QueryCustomEntityAsync(GetCustomEntityRenderDetailsByIdQuery query, IExecutionContext executionContext)
    {
        CustomEntityVersion result;

        if (query.PublishStatus == PublishStatusQuery.SpecificVersion)
        {
            if (!query.CustomEntityVersionId.HasValue)
            {
                throw new Exception("A CustomEntityVersionId must be included in the query to use PublishStatusQuery.SpecificVersion");
            }

            result = await _dbContext
                .CustomEntityVersions
                .AsNoTracking()
                .Include(e => e.CustomEntity)
                .FilterActive()
                .FilterByCustomEntityId(query.CustomEntityId)
                .FilterByCustomEntityVersionId(query.CustomEntityVersionId.Value)
                .SingleOrDefaultAsync();
        }
        else
        {
            var dbResult = await _dbContext
                .CustomEntityPublishStatusQueries
                .AsNoTracking()
                .Include(e => e.CustomEntityVersion)
                .ThenInclude(e => e.CustomEntity)
                .FilterActive()
                .FilterByCustomEntityId(query.CustomEntityId)
                .FilterByStatus(query.PublishStatus, executionContext.ExecutionDate)
                .SingleOrDefaultAsync();

            result = dbResult?.CustomEntityVersion;
        }

        return result;
    }

    private ICollection<string> MapPageRoutings(
        ICollection<PageRoutingInfo> allRoutings,
        CustomEntityVersion dbResult
        )
    {
        if (allRoutings == null) return Array.Empty<string>();

        var urls = new List<string>(allRoutings.Count());

        foreach (var detailsRouting in allRoutings
            .Where(r => r.CustomEntityRouteRule != null))
        {
            var detailsUrl = detailsRouting
                .CustomEntityRouteRule
                .MakeUrl(detailsRouting.PageRoute, detailsRouting.CustomEntityRoute);

            urls.Add(detailsUrl);
        }

        return urls;
    }
}
