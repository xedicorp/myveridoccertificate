namespace VeriDocCertificate.CofoundaryCMS;

/// <summary>
/// This mapper is required to map from the data returned from the database to
/// a strongly typed model that we can use in the view template. This might seem
/// a little verbose but this allows us to use a strongly typed model in the view
/// and provides us with a lot of flexibility when mapping from unstructured data
/// </summary>
public class SeoToolsDisplayModelMapper
    : ICustomEntityDisplayModelMapper<SeoToolsDataModel, SeoToolsDisplayModel>
{
    private readonly IContentRepository _contentRepository;

    public SeoToolsDisplayModelMapper(
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
    public async Task<SeoToolsDisplayModel> MapDisplayModelAsync(CustomEntityRenderDetails renderDetails, SeoToolsDataModel dataModel, PublishStatusQuery publishStatusQuery)
    {
        var displayModel = new SeoToolsDisplayModel()
        {
           GoogleAnalytics = dataModel.GoogleAnalytics,
           BingVerification = dataModel.BingVerification,


           GoogleVerification = dataModel.GoogleVerification,


           GoogleTagManager = dataModel.GoogleTagManager,


        };
        return displayModel;
    }

    

    
}