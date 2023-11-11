using System.ComponentModel.DataAnnotations;
using VeriDocCertificate.CofoundaryCMS.Models.Enum;

namespace Cofoundry.Web;

/// <summary>
/// Data model representing a quotation with optional citation and title
/// </summary>
/// 

//Background Options



public class FfbDetailsDataModel : IPageBlockTypeDataModel
{

    [Required]
    [PreviewTitle]
    public string FfbTitle { get; set; }

  
    [Display(Description = "Image to display in the banner")]
    [Document(FileExtensions = new string[] { "svg", "webp", "jpg", "jpeg", "png" })]
    public int? FfbImageID { get; set; }
 
    [Required]
    [SelectList(typeof(MediaType))]
    public string MeidaLookup { get; set; }


    [SelectList(typeof(VideoType))]
    public string VideoLookup { get; set; }


    [Display(Description = "Video to display in the banner")]
    public string FfbIframe { get; set; }

    [Display(Description = "Formatted text to display in the Banner.")]
    [Required]
    [Html(HtmlToolbarPreset.Headings, HtmlToolbarPreset.Custom, CustomToolbar = "undo redo | bold italic underline | numlist | link unlink | removeformat | code ")]
    public string FfbDescription { get; set; }


}