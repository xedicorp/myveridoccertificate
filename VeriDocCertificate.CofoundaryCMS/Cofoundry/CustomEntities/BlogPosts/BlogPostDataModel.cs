using Cofoundry.Domain.Data;
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
public class BlogPostDataModel : ICustomEntityDataModel
{
    [Number]
    [Required]
    [Display(Description = "Sort order is used to do sorting.")]
    public int SortOrder { get; set; }

    [MaxLength(1000)]
    [Required]
    [Display(Description = "A description for display in search results and in the details page meta description.")]
    [MultiLineText]
    public string ShortDescription { get; set; }

    [Document(FileExtensions = new string[] { "svg", "webp", "jpg", "jpeg", "png" })]
    [Display(Name = "Thumbnail Image", Description = "Square image that displays against the blog in the listing page.")]
    public int ThumbnailImageAssetId { get; set; }



    [Display(Name = "Categories", Description = "Drag and drop to customize the category ordering.")]
    [CustomEntityCollection(CategoryCustomEntityDefinition.DefinitionCode, IsOrderable = true)]
    [Required]
    public ICollection<int> CategoryIds { get; set; }






    [MaxLength(100)]
    [Display(Description = "Title for display in search results and in the details page meta description.")]

    public string MetaTitle { get; set; }
    [MaxLength(100)]
    [Display(Description = "A description for display in search results and in the details page meta description.")]
    public string MetaDescription { get; set; }

    [MaxLength(100)]
    [Display(Description = "Some Keywords for display in search results and in the details page meta description.")]
    public string MetaKeywords { get; set; }
    [Display(Description = "Title for display in search results and in the details page meta description.")]
    public string OpenGraphTitle { get; set; }

    [Display(Description = "Title for display in search results and in the details page meta description.")]

    public string OpenGraphDescription { get; set; }

    [Image]
    [Display(Name = "OpenGraph Image", Description = "Square image that displays against the blog in the listing page.")]
     public int OpenGraphImageAssetId { get; set; }
    

}