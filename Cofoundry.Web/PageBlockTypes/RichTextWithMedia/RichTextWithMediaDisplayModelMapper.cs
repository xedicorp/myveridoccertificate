﻿using Microsoft.AspNetCore.Html;

namespace Cofoundry.Web;

public class RichTextWithMediaDisplayModelMapper : IPageBlockTypeDisplayModelMapper<RichTextWithMediaDataModel>
{
    public Task MapAsync(
        PageBlockTypeDisplayModelMapperContext<RichTextWithMediaDataModel> context,
        PageBlockTypeDisplayModelMapperResult<RichTextWithMediaDataModel> result
        )
    {
        foreach (var item in context.Items)
        {
            var displayModel = new RichTextWithMediaDisplayModel();
            displayModel.RawHtml = new HtmlString(item.DataModel.RawHtml);

            result.Add(item, displayModel);
        }

        return Task.CompletedTask;
    }
}
