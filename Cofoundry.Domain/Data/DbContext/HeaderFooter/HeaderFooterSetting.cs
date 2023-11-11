using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cofoundry.Domain.Data { 
    public  class HeaderFooterSetting
    {
        [Key]
        public int SettingId { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public int? HeaderLogoImageId { get; set; }
       
        public int? FooterLogoImageId { get; set; }
        
        public int? PartnerLogoImageId { get; set; }
        public string HeaderMenuLinks { get; set; }
        public string UsefulMenuLinks { get; set; }
        public string SocialMediaLinks { get; set; }
    }
}
