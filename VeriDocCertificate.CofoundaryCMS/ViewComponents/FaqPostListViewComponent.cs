using Microsoft.AspNetCore.Mvc;

namespace VeriDocCertificate.CofoundaryCMS;

public class FaqPostListViewComponent : ViewComponent
{
    private readonly IContentRepository _contentRepository;
    private readonly IVisualEditorStateService _visualEditorStateService;

    public FaqPostListViewComponent(
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
        // determine whether to show unpublished blog posts in the list.
        var visualEditorState = await _visualEditorStateService.GetCurrentAsync();
        var ambientEntityPublishStatusQuery = visualEditorState.GetAmbientEntityPublishStatusQuery();

        var query = new SearchCustomEntityRenderSummariesQuery()
        {
            CustomEntityDefinitionCode = FaqPostCustomEntityDefinition.DefinitionCode,
            
        };

        // TODO: Filtering by Category (webQuery.CategoryId)
        // Searching/filtering custom entities is not implemented yet, but it
        // is possible to build your own search index using the message handling
        // framework or writing a custom query against the UnstructuredDataDependency table
        // See issue https://github.com/cofoundry-cms/cofoundry/issues/12

        var entities = await _contentRepository
            .CustomEntities()
            .Search()
            .AsRenderSummaries(query)
            .ExecuteAsync();

        var viewModel = await MapFaqPostsAsync(entities, ambientEntityPublishStatusQuery);

        return View(viewModel);
    }

    /// <summary>
    /// ModelBinder is not supported in view components so we have to bind
    /// this manually. We have an issue open to try and improve the experience here
    /// https://github.com/cofoundry-cms/cofoundry/issues/125
    /// </summary>
    

    /// <summary>
    /// Here we map the raw custom entity data from Cofoundry into our
    /// own FaqPostSummary which will get sent to be rendered in the 
    /// view.
    /// 
    /// This code is repeated in HomepageFaqPostsViewComponent for 
    /// simplicity, but typically you'd put the code into a shared 
    /// mapper or break data access out into it's own shared layer.
    /// </summary>
    private async Task<PagedQueryResult<FaqPostSummary>> MapFaqPostsAsync(
        PagedQueryResult<CustomEntityRenderSummary> customEntityResult,
        PublishStatusQuery ambientEntityPublishStatusQuery
        )
    {
        var blogPosts = new List<FaqPostSummary>(customEntityResult.Items.Count());

        var imageAssetIds = customEntityResult
            .Items
            .Select(i => (FaqPostDataModel)i.Model)
            .Select(m => m.ThumbnailImageAssetId)
            .Distinct();

       

        var imageLookup = await _contentRepository
            .DocumentAssets()
            .GetByIdRange(imageAssetIds.OfType<int>())
            .AsRenderDetails()
            .ExecuteAsync();

        

        foreach (var customEntity in customEntityResult.Items)
        {
            var model = (FaqPostDataModel)customEntity.Model;

            var blogPost = new FaqPostSummary()
            {
                Title = customEntity.Title,
                ShortDescription = model.ShortDescription,
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