using Cofoundry.Core;
using Cofoundry.Domain;
using Cofoundry.Domain.Internal;
using Microsoft.AspNetCore.Html;
using static System.Net.Mime.MediaTypeNames;

namespace Cofoundry.Web;

public class TryNowCTADisplayModelMapper : IPageBlockTypeDisplayModelMapper<TryNowCTADataModel>
{
    private readonly IContentRepository _contentRepository;
    private IImageAssetRouteLibrary _imageAssetRouteLibrary;
    public TryNowCTADisplayModelMapper(IImageAssetRouteLibrary imageAssetRouteLibrary, IContentRepository contentRepository )
    {
        _imageAssetRouteLibrary = imageAssetRouteLibrary;
        _contentRepository = contentRepository;
    }
    public Task MapAsync(PageBlockTypeDisplayModelMapperContext<TryNowCTADataModel> context, PageBlockTypeDisplayModelMapperResult<TryNowCTADataModel> result)
    {
        foreach (var item in context.Items)
        {
            var displayModel = new TryNowCTADisplayModel();

            
            //Normal Text
            displayModel.Title = new HtmlString(HtmlFormatter.ConvertLineBreaksToBrTags(item.DataModel.Title));

            displayModel.LinkBtn = item.DataModel.LinkBtn;
            //Normal Text End
            result.Add(item, displayModel);
        }
        return Task.CompletedTask;
    }
}