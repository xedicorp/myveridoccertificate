using System.ComponentModel.DataAnnotations;

namespace Cofoundry.Web;

/// <summary>
/// Data model representing a quotation with optional citation and title
/// </summary>
/// 

public class FreeTrialBannerDataModel : IPageBlockTypeDataModel
{

    [Display(Description = "Image to display in the banner")]
    [Required]
    [Document(FileExtensions = new string[] { "svg", "webp", "jpg", "jpeg", "png" })]
    public int BannerImageID { get; set; }


    [Display(Description = "Formatted text to display in the Banner.")]
    [Required]
    [Html(HtmlToolbarPreset.Headings, HtmlToolbarPreset.Custom, CustomToolbar = "undo redo | bold italic underline | numlist | link unlink | code ")]
    public string BannerDescription { get; set; }


}