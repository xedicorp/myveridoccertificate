using Cofoundry.Core;
using Cofoundry.Domain;
using Cofoundry.Domain.Internal;
using Microsoft.AspNetCore.Html;
using static System.Net.Mime.MediaTypeNames;

namespace Cofoundry.Web;

public class CustomHTMLDisplayModelMapper : IPageBlockTypeDisplayModelMapper<CustomHTMLDataModel>
{
    private readonly IContentRepository _contentRepository;
    private IImageAssetRouteLibrary _imageAssetRouteLibrary;
    public CustomHTMLDisplayModelMapper(IImageAssetRouteLibrary imageAssetRouteLibrary, IContentRepository contentRepository )
    {
        _imageAssetRouteLibrary = imageAssetRouteLibrary;
        _contentRepository = contentRepository;
    }
    public async Task MapAsync(PageBlockTypeDisplayModelMapperContext<CustomHTMLDataModel> context, PageBlockTypeDisplayModelMapperResult<CustomHTMLDataModel> result)
    {


        foreach (var item in context.Items)
        {
            var displayModel = new CustomHTMLDisplayModel();
            //section name
            displayModel.SectionName = item.DataModel.SectionName;
           
            //Pick HTML Editor
            displayModel.SectionHTML = new HtmlString(HtmlFormatter.ConvertLineBreaksToBrTags(item.DataModel.SectionHTML));
            //HTML End

            result.Add(item, displayModel);
        }

        
    }
   
}