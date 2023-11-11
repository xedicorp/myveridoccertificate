using System.ComponentModel.DataAnnotations;

namespace VeriDocCertificate.CofoundaryCMS;

/// <summary>
/// This defines the custom data that gets stored with each blog post. Data
/// is stored in an unstructured format (json) so simple data types are 
/// best. For associations, you just need to store the key of the relation.
/// 
/// Attributes can be used to describe validations as well as hints to the 
/// content editor UI on how to render the data input controls.
/// </summary>
public class SeoToolsDataModel : ICustomEntityDataModel
{
    [MaxLength(20)]
    [Required]
    [Display(Description = "Google Analytics Code For Website")]
    public string GoogleAnalytics { get; set; }

    [MaxLength(50)]
    [Required]
    [Display(Description = "Bing verification Code For Website")]
    public string BingVerification { get; set; }

    [MaxLength(50)]
    [Required]
    [Display(Description = "Google verification Code For Website")]
    public string GoogleVerification { get; set; }

    [MaxLength(20)]
    [Required]
    [Display(Description = "Google tag manager Code For Website")]
    public string GoogleTagManager { get; set; }

    
}