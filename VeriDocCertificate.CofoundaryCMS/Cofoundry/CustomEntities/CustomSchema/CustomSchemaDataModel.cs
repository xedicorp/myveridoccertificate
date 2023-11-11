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
public class CustomSchemaDataModel : ICustomEntityDataModel
{
    
    [Display(Name="ID", Description = "Schema url")]
   
    public string Id { get; set; }
    [Display(Name="Name", Description = "Schema Name")]

    public string Name { get; set; }

    //[Image(MinWidth = 300)]
    //[Display(Name = "Icon", Description = "Icon image that displays against the social media link.")]
    //public int? ThumbnailImageAssetId { get; set; }

    //public string IconImageUrl { get; set; }

    //public string ClassName { get; set; }
}