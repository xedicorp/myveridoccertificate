using System.ComponentModel.DataAnnotations;

namespace VeriDocCertificate.CofoundaryCMS;

public class FeatureContentListDataModel : INestedDataModel
{
    
    [Display(Description = "Image to display in the Features")]
    [Required]
    [Document(FileExtensions = new string[] { "svg", "webp", "jpg", "jpeg", "png" })]
    public int FeatureImageID { get; set; }

    [PreviewTitle]
    [Display(Description = "Title to display in the slide.")]
    [MaxLength(50)]
    public string FeatureTitle { get; set; }
   

    [Display(Description = "Formatted text to display in the slide.")]
    [Required]
    [Html(HtmlToolbarPreset.Headings, HtmlToolbarPreset.Custom, CustomToolbar = "undo redo | bold italic underline | numlist | link unlink | code ")]

    public string FeatureDescription { get; set; }
}
