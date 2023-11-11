using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cofoundry.Domain.Domain.HeaderFooter.Commands
{
    public  class SaveHeaderFooterCommand : ICommand, ILoggableCommand
        //, IValidatableObject
    {
        
         
        /// <summary>
        /// The description of the content of the page. This is intended to
        /// be used in the description html meta tag.
        /// </summary>
        [Display(Name = "Address" )]
        [StringLength(300)]
        public string Address { get; set; }
        /// <summary>
        /// The description of the content of the page. This is intended to
        /// be used in the description html meta tag.
        /// </summary>
        [Display(Name = "Email")]
        [StringLength(300)]
        public string Email { get; set; }

        /// <summary>
        /// Indicates whether the page should show in the auto-generated site map
        /// that gets presented to search engine robots.
        /// </summary>
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Document(FileExtensions = new string[] { "svg", "webp", "jpg", "jpeg", "png" })]
        [PositiveInteger]
        public int? HeaderLogoImageId { get; set; }
        [Document(FileExtensions = new string[] { "svg", "webp", "jpg", "jpeg", "png" })]
        [PositiveInteger]
        public int? FooterLogoImageId { get; set; }
        [Document(FileExtensions = new string[] { "svg", "webp", "jpg", "jpeg", "png" })]
        [PositiveInteger]
        public int? PartnerLogoImageId { get; set; }
        public string HIds { get; set; }
        public string UIds { get; set; }
        public string SIds { get; set; }

       
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{   
        //    if (!string.IsNullOrEmpty(Address))
        //    {
        //        yield return new ValidationResult("Custom entity details pages should not specify a Url Path, instead they should specify a Routing Rule.", new[] { nameof(Address) });
        //    }
        //    //if (PageType == PageType.CustomEntityDetails)
        //    //{
        //    //    if (string.IsNullOrWhiteSpace(CustomEntityRoutingRule))
        //    //    {
        //    //        yield return new ValidationResult("A routing rule is required for custom entity details page types.", new[] { nameof(CustomEntityRoutingRule) });
        //    //    }
        //    //    if (!string.IsNullOrEmpty(UrlPath))
        //    //    {
        //    //        yield return new ValidationResult("Custom entity details pages should not specify a Url Path, instead they should specify a Routing Rule.", new[] { nameof(UrlPath) });
        //    //    }
        //    //}
        //}
    }
}
