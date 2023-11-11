using Microsoft.AspNetCore.Mvc;
using VeriDocCertificate.CofoundaryCMS.Models;

namespace VeriDocCertificate.CofoundaryCMS;

public class FeaturePostFilterByCategoriesViewComponent : ViewComponent
{
    private readonly IContentRepository _contentRepository;
    private readonly IVisualEditorStateService _visualEditorStateService;

    public FeaturePostFilterByCategoriesViewComponent(
        IContentRepository contentRepository,
        IVisualEditorStateService visualEditorStateService
        )
    {
        _contentRepository = contentRepository;
        _visualEditorStateService = visualEditorStateService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // We can use the current visual editor state (e.g. edit mode, live mode) to
        // determine whether to show unpublished categories in the list.
        var webQuery = ModelBind();

        // We can use the current visual editor state (e.g. edit mode, live mode) to
        // determine whether to show unpublished blog posts in the list.
        var visualEditorState = await _visualEditorStateService.GetCurrentAsync();
        var ambientEntityPublishStatusQuery = visualEditorState.GetAmbientEntityPublishStatusQuery();

        var catquery = new SearchCustomEntityRenderSummariesQuery()
        {
            CustomEntityDefinitionCode = FeatureCategoryCustomEntityDefinition.DefinitionCode,
            PageNumber = webQuery.PageNumber,
            PageSize = 100,
            PublishStatus = ambientEntityPublishStatusQuery
        };
        var postquery = new SearchCustomEntityRenderSummariesQuery()
        {
            CustomEntityDefinitionCode = FeaturePostCustomEntityDefinition.DefinitionCode,
            PageNumber = webQuery.PageNumber,
            PageSize = 100,
            PublishStatus = ambientEntityPublishStatusQuery
        };

        var pagedCategories = await _contentRepository
            .CustomEntities()
            .Search()
            .AsRenderSummaries(catquery)
            .MapItem(MapCategory)
            .ExecuteAsync();
        

        // TODO: Filtering by Category (webQuery.CategoryId)
        // Searching/filtering custom entities is not implemented yet, but it
        // is possible to build your own search index using the message handling
        // framework or writing a custom query against the UnstructuredDataDependency table
        // See issue https://github.com/cofoundry-cms/cofoundry/issues/12

        var entities = await _contentRepository
            .CustomEntities()
            .Search()
            .AsRenderSummaries(postquery)
            .ExecuteAsync();

        var viewModel = await MapFeaturePostsAsync(entities, ambientEntityPublishStatusQuery);
        var modelCollection = new FeatureListVM();
        modelCollection.FeatureCategories = pagedCategories.Items;
        modelCollection.FeaturePostModel = viewModel;
  

       

        return View(modelCollection);

    }
    private SearchFeaturePostsQuery ModelBind()
    {
        var webQuery = new SearchFeaturePostsQuery();
        webQuery.PageNumber = IntParser.ParseOrDefault(Request.Query[nameof(webQuery.PageNumber)]);
        webQuery.PageSize = IntParser.ParseOrDefault(Request.Query[nameof(webQuery.PageSize)]);
        webQuery.CategoryId = IntParser.ParseOrDefault(Request.Query[nameof(webQuery.CategoryId)]);

        return webQuery;
    }
    private FeatureCategorySummary MapCategory(CustomEntityRenderSummary customEntity)
    {
        var model = (FeatureCategoryDataModel)customEntity.Model;

        var category = new FeatureCategorySummary()
        {
            CategoryId = customEntity.CustomEntityId,
            Title = customEntity.Title,
            ShortDescription = model.ShortDescription,
            SortOrder = model.SortOrder
        };

        return category;
    }
    private async Task<PagedQueryResult<FeaturePostSummary>> MapFeaturePostsAsync(
       PagedQueryResult<CustomEntityRenderSummary> customEntityResult,
       PublishStatusQuery ambientEntityPublishStatusQuery
       )
    {
        var blogPosts = new List<FeaturePostSummary>(customEntityResult.Items.Count());

        var imageAssetIds = customEntityResult
            .Items
            .Select(i => (FeaturePostDataModel)i.Model)
            .Select(m => m.ThumbnailImageAssetId)
            .Distinct();



        var imageLookup = await _contentRepository
            .DocumentAssets()
            .GetByIdRange(imageAssetIds.OfType<int>())
            .AsRenderDetails()
            .ExecuteAsync();


        var iconAssetIds = customEntityResult
            .Items
            .Select(i => (FeaturePostDataModel)i.Model)
            .Select(m => m.ThumbnailIconId)
            .Distinct();



        var iconLookup = await _contentRepository
            .DocumentAssets()
            .GetByIdRange(iconAssetIds.OfType<int>())
            .AsRenderDetails()
            .ExecuteAsync();


        foreach (var customEntity in customEntityResult.Items)
        {
            var model = (FeaturePostDataModel)customEntity.Model;

            var blogPost = new FeaturePostSummary()
            {
                Title = customEntity.Title,
                ShortDescription = model.ShortDescription,
                ThumbnailIcon = iconLookup.GetOrDefault(model.ThumbnailIconId),
                Categorylist = model.CategoryIds.FirstOrDefault(),
                MeidaLookup = model.MeidaLookup,
                VideoLookup = model.VideoLookup,
                ThumbnailImageAsset = imageLookup.GetOrDefault(model.ThumbnailImageAssetId),
                ThumbnailIframeId = model.ThumbnailIframeId,
                FullPath = customEntity.PageUrls.FirstOrDefault(),
                PostDate = customEntity.PublishDate,
                SortOrder = model.SortOrder
            };



            blogPosts.Add(blogPost);
        }
        blogPosts = blogPosts.OrderBy(p => p.SortOrder).ToList();
        return customEntityResult.ChangeType(blogPosts);
    }
}
