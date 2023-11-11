using System.ComponentModel.DataAnnotations;
using VeriDocCertificate.CofoundaryCMS.Models.Enum;

namespace Cofoundry.Web;

/// <summary>
/// Data model representing a quotation with optional citation and title
/// </summary>
/// 

//Background Options


public class DefaultContentDataModel : IPageBlockTypeDataModel
{

    [Required]
    [PreviewTitle]
    public string SectionName { get; set; }

    [RadioList(typeof(ColorOption))]
    public ColorOption BackgroundColor { get; set; }

    [Display(Description = "Title to display in the Title.")]
    [MaxLength(200)]
    [MultiLineText(Rows = 4)]
    public string SectionTitle { get; set; }

    public string SectionSubTitle { get; set; }

    [Required]
    [RadioList(typeof(OrientationOption))]
    public OrientationOption Orientation { get; set; }


   
    [Display(Description = "Image to display in the banner")]
    [Required]
    [Document(FileExtensions = new string[] { "svg", "webp", "jpg", "jpeg", "png" })]
    public int SectionImageID { get; set; }


    [Display(Description = "Formatted text to display in the Banner.")]
    [Required]
    [Html(HtmlToolbarPreset.Headings, HtmlToolbarPreset.Custom, CustomToolbar = "undo redo | bold italic underline | numlist | link unlink | code ")]
    public string SectionDescription { get; set; }


}