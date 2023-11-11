using Cofoundry.Core;
using Cofoundry.Domain;
using Cofoundry.Domain.Internal;
using Microsoft.AspNetCore.Html;
using static System.Net.Mime.MediaTypeNames;

namespace Cofoundry.Web;

public class GreenBgCTADisplayModelMapper : IPageBlockTypeDisplayModelMapper<GreenBgCTADataModel>
{
    private readonly IContentRepository _contentRepository;
    private IImageAssetRouteLibrary _imageAssetRouteLibrary;
    public GreenBgCTADisplayModelMapper(IImageAssetRouteLibrary imageAssetRouteLibrary, IContentRepository contentRepository )
    {
        _imageAssetRouteLibrary = imageAssetRouteLibrary;
        _contentRepository = contentRepository;
    }
    public Task MapAsync(PageBlockTypeDisplayModelMapperContext<GreenBgCTADataModel> context, PageBlockTypeDisplayModelMapperResult<GreenBgCTADataModel> result)
    {
        foreach (var item in context.Items)
        {
            var displayModel = new GreenBgCTADisplayModel();

            
            //Normal Text
            displayModel.Title = item.DataModel.Title;

            displayModel.LinkBtn = item.DataModel.LinkBtn;
            //Normal Text End
            result.Add(item, displayModel);
        }
        return Task.CompletedTask;
    }
}