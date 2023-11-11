namespace VeriDocCertificate.CofoundaryCMS;

/// <summary>
/// This mapper is required to map from the data returned from the database to
/// a strongly typed model that we can use in the view template. This might seem
/// a little verbose but this allows us to use a strongly typed model in the view
/// and provides us with a lot of flexibility when mapping from unstructured data
/// </summary>
public class FeaturePostDisplayModelMapper
    : ICustomEntityDisplayModelMapper<FeaturePostDataModel, FeaturePostDisplayModel>
{
    private readonly IContentRepository _contentRepository;

    public FeaturePostDisplayModelMapper(
        IContentRepository contentRepository
        )
    {
        _contentRepository = contentRepository;
    }

    /// <summary>
    /// Maps a raw custom entity data model to a display model that can be rendered out 
    /// to a view template.
    /// </summary>
    /// <param name="renderDetails">
    /// The raw custom entity data pulled from the database.
    /// </param>
    /// <param name="dataModel">
    /// Typed model data to map from. This is the same model that's in the render 
    /// details model, but is passed in as a typed model for easy access.
    /// </param>
    /// <param name="publishStatusQuery">
    /// The query type that should be used to query dependent entities. E.g. if the custom
    /// entity was queried with the Published status query, then any dependent entities should
    /// also be queried as Published.
    /// </param>
    public async Task<FeaturePostDisplayModel> MapDisplayModelAsync(
        CustomEntityRenderDetails renderDetails,
        FeaturePostDataModel dataModel,
        PublishStatusQuery publishStatusQuery
        )
    {
        var displayModel = new FeaturePostDisplayModel()
        {
            MetaDescription = dataModel.ShortDescription,
            PageTitle = renderDetails.Title,
            Title = renderDetails.Title,
            Date = renderDetails.CreateDate,
            FullPath = renderDetails.PageUrls.FirstOrDefault()
        };

        displayModel.FeatureCategories = await MapCategories(dataModel, publishStatusQuery);


        return displayModel;
    }

    private async Task<ICollection<FeatureCategorySummary>> MapCategories(
        FeaturePostDataModel dataModel,
        PublishStatusQuery publishStatusQuery
        )
    {
        if (EnumerableHelper.IsNullOrEmpty(dataModel.CategoryIds))
        {
            return Array.Empty<FeatureCategorySummary>();
        }

        // We manually query and map relations which gives us maximum flexibility when
        // mapping models. Cofoundry provides apis and extensions to make this easier.
        var results = await _contentRepository
            .CustomEntities()
            .GetByIdRange(dataModel.CategoryIds)
            .AsRenderSummaries(publishStatusQuery)
            .FilterAndOrderByKeys(dataModel.CategoryIds)
            .MapItem(MapCategory)
            .ExecuteAsync();

        return results;
    }

    /// <summary>
    /// We could use AutoMapper here, but to keep it simple let's just do manual mapping.
    /// </summary>
    private FeatureCategorySummary MapCategory(CustomEntityRenderSummary renderSummary)
    {
        // A CustomEntityRenderSummary will always contain the data model for the custom entity 
        var model = renderSummary.Model as FeatureCategoryDataModel;

        var category = new FeatureCategorySummary()
        {
            CategoryId = renderSummary.CustomEntityId,
            Title = renderSummary.Title,
            ShortDescription = model?.ShortDescription
        };

        return category;
    }


}