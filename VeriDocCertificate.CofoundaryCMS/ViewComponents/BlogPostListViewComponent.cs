using Microsoft.AspNetCore.Mvc;

namespace VeriDocCertificate.CofoundaryCMS;

public class BlogPostListViewComponent : ViewComponent
{
    private readonly IContentRepository _contentRepository;
    private readonly IVisualEditorStateService _visualEditorStateService;

    public BlogPostListViewComponent(
        IContentRepository contentRepository,
        IVisualEditorStateService visualEditorStateService
        )
    {
        _contentRepository = contentRepository;
        _visualEditorStateService = visualEditorStateService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var webQuery = ModelBind();

        // We can use the current visual editor state (e.g. edit mode, live mode) to
        // determine whether to show unpublished blog posts in the list.
        var visualEditorState = await _visualEditorStateService.GetCurrentAsync();
        var ambientEntityPublishStatusQuery = visualEditorState.GetAmbientEntityPublishStatusQuery();

        var query = new SearchCustomEntityRenderSummariesQuery()
        {
            CustomEntityDefinitionCode = BlogPostCustomEntityDefinition.DefinitionCode,
            PageNumber = webQuery.PageNumber,
            PageSize = 100,
            PublishStatus = ambientEntityPublishStatusQuery
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

        var viewModel = await MapBlogPostsAsync(entities, ambientEntityPublishStatusQuery);

        return View(viewModel);
    }

    /// <summary>
    /// ModelBinder is not supported in view components so we have to bind
    /// this manually. We have an issue open to try and improve the experience here
    /// https://github.com/cofoundry-cms/cofoundry/issues/125
    /// </summary>
    private SearchBlogPostsQuery ModelBind()
    {
        var webQuery = new SearchBlogPostsQuery();
        webQuery.PageNumber = IntParser.ParseOrDefault(Request.Query[nameof(webQuery.PageNumber)]);
        webQuery.PageSize = IntParser.ParseOrDefault(Request.Query[nameof(webQuery.PageSize)]);
        webQuery.CategoryId = IntParser.ParseOrDefault(Request.Query[nameof(webQuery.CategoryId)]);

        return webQuery;
    }

    /// <summary>
    /// Here we map the raw custom entity data from Cofoundry into our
    /// own BlogPostSummary which will get sent to be rendered in the 
    /// view.
    /// 
    /// This code is repeated in HomepageBlogPostsViewComponent for 
    /// simplicity, but typically you'd put the code into a shared 
    /// mapper or break data access out into it's own shared layer.
    /// </summary>
    private async Task<PagedQueryResult<BlogPostSummary>> MapBlogPostsAsync(
        PagedQueryResult<CustomEntityRenderSummary> customEntityResult,
        PublishStatusQuery ambientEntityPublishStatusQuery
        )
    {
        var blogPosts = new List<BlogPostSummary>(customEntityResult.Items.Count());

        var imageAssetIds = customEntityResult
            .Items
            .Select(i => (BlogPostDataModel)i.Model)
            .Select(m => m.ThumbnailImageAssetId)
            .Distinct();

       

        var imageLookup = await _contentRepository
            .DocumentAssets()
            .GetByIdRange(imageAssetIds)
            .AsRenderDetails()
            .ExecuteAsync();

        

        foreach (var customEntity in customEntityResult.Items)
        {
            var model = (BlogPostDataModel)customEntity.Model;

            var blogPost = new BlogPostSummary()
            {
                Title = customEntity.Title,
                ShortDescription = model.ShortDescription,
                Categorylist = model.CategoryIds.ToList(),
                ThumbnailImageAsset = imageLookup.GetOrDefault(model.ThumbnailImageAssetId),
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