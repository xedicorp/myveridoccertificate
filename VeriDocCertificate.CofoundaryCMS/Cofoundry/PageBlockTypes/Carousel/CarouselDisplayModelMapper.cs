using Cofoundry.Core;
using Microsoft.AspNetCore.Html;

namespace VeriDocCertificate.CofoundaryCMS;

public class CarouselDisplayModelMapper : IPageBlockTypeDisplayModelMapper<CarouselDataModel>
{
    private readonly IContentRepository _contentRepository;

    public CarouselDisplayModelMapper(
        IContentRepository contentRepository
        )
    {
        _contentRepository = contentRepository;
    }

    public async Task MapAsync(
        PageBlockTypeDisplayModelMapperContext<CarouselDataModel> context,
        PageBlockTypeDisplayModelMapperResult<CarouselDataModel> result
        )
    {
        
        // Map display model
        foreach (var items in context.Items)
        {
            var output = new CarouselDisplayModel();

            output.Lists = EnumerableHelper
                .Enumerate(items.DataModel.Lists)
                .Select(m => new CarouselListDisplayModel()
                {
                    
                    CarouselText = new HtmlString(HtmlFormatter.ConvertLineBreaksToBrTags(m.CarouselText)),
                    CarouselAuthor = m.CarouselAuthor,
             
                }).ToList();

            result.Add(items, output);
        }
    }
}
