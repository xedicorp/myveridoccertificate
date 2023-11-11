using System.ComponentModel.DataAnnotations;
using VeriDocCertificate.CofoundaryCMS.Models.Enum;

namespace Cofoundry.Web;

/// <summary>
/// Data model representing a quotation with optional citation and title
/// </summary>
/// 

//Background Options


public class CustomHTMLDataModel : IPageBlockTypeDataModel
{

    [Required]
    [PreviewTitle]
    public string SectionName { get; set; }

 
    [Display(Description = "Formatted text to display in the Banner.")]
    [Required]
    [Html(HtmlToolbarPreset.Headings, HtmlToolbarPreset.Custom, CustomToolbar = "undo redo | bold italic underline | numlist | link unlink | code ")]
    public string SectionHTML { get; set; }


}