using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cofoundry.Domain.Domain.HeaderFooter.Models
{
    public class HeaderFooterDetails
    { 
        public string Address { get; set; } 
        public string Email { get; set; } 
        public string Phone { get; set; }
        public int? HeaderLogoImageId { get; set; }
        public int? FooterLogoImageId { get; set; }
        public int? PartnerLogoImageId { get; set; }
        public List<HeaderMenuItem> HeaderMenuItems { get; set; }
        public List<HeaderMenuItem> UsefulLinks { get; set; }
        public List<HeaderMenuItem> SocialMediaIcons { get; set; }
        public string HeaderMenuItemsIds { get; set; }
        public string UsefulLinksIds { get; set; }
        public string SocialMediaIconsIds { get; set; }
        public string HeaderLogoImageUrl { get; set; }
        public string FooterLogoImageUrl { get; set; }
        public string PartnerLogoImageUrl { get; set; }
    }
    public class HeaderMenuItem
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public int CustomEntityId { get; set; }
        public string ClassName { get; set; }

    }
    public class CustomHeaderMenuItem
    {
        
        public string Url { get; set; }
        public string ClassName { get; set; }

    }

}
