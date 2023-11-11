using System.ComponentModel.DataAnnotations;

namespace Cofoundry.Web;

/// <summary>
/// Data model representing a quotation with optional citation and title
/// </summary>
public class GreenBgCTADataModel : IPageBlockTypeDataModel
{


    [PreviewTitle]
    [Required]
    [Display(Description = "Title to display in the slide.")]
    [MaxLength(200)]
    public string Title { get; set; }

    [Display(Description = "Linked CTA Button.")]
    public string LinkBtn { get; set; }

}