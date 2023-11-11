using System.ComponentModel.DataAnnotations;

namespace VeriDocCertificate.CofoundaryCMS;

public class CarouselListDataModel : INestedDataModel
{
    

   

    [Display(Description = "Formatted text to display in the slide.")]
    [Required]
    [Html(HtmlToolbarPreset.Headings, HtmlToolbarPreset.Custom, CustomToolbar = "undo redo | bold italic underline | numlist | link unlink | removeformat | code ")]
    public string CarouselText { get; set; }

    public string CarouselAuthor { get; set; }
}
