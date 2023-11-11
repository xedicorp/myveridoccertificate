using System.ComponentModel.DataAnnotations;
using VeriDocCertificate.CofoundaryCMS.Models.Enum;

namespace Cofoundry.Web;

/// <summary>
/// Data model representing a quotation with optional citation and title
/// </summary>
public class TryNowCTADataModel : IPageBlockTypeDataModel
{

    [Required]
    [PreviewTitle]
    public string SectionName { get; set; }

    [RadioList(typeof(ColorOption))]
    public ColorOption BackgroundColor { get; set; }

    [Display(Description = "Formatted text to display in the Banner.")]
    [Required]
    [Html(HtmlToolbarPreset.Headings, HtmlToolbarPreset.Custom, CustomToolbar = "undo redo | bold italic underline | numlist | link unlink | code ")]
    public string Title { get; set; }

    [Display(Description = "Linked CTA Button.")]
    public string LinkBtn { get; set; }

}