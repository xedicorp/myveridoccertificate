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
public class SocialMediaLinkDataModel : ICustomEntityDataModel
{
    
    [Display(Description = "A short description that appears as a tooltip when hovering over the category.")]
   
    public string URL { get; set; }

    //[Image(MinWidth = 300)]
    //[Display(Name = "Icon", Description = "Icon image that displays against the social media link.")]
    //public int? ThumbnailImageAssetId { get; set; }

    //public string IconImageUrl { get; set; }
    [Display(Name="CSS Class Name", Description = "A short description that appears as a tooltip when hovering over the category.")]

    public string ClassName { get; set; }
}