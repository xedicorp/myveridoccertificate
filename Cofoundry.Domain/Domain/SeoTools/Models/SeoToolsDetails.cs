using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cofoundry.Domain.Domain.SeoTools.Models
{
    public class SeoToolsDetails
    { 
        public string ViewPort { get; set; } 
        public string CopyRight { get; set; } 
        public string Author { get; set; }
        public string ReplyTo { get; set; } 
        public string Robots { get; set; }
        public string ContentLanguage { get; set; }
        public string Audience { get; set; }
        public string RevisitAfter { get; set; }
        public string Distribution { get; set; }
        public string AltDistribution { get; set; } 
        public string Publisher { get; set; }
        public string AltCopyRight { get; set; }
        public string Rel { get; set; }
        public string Href { get; set; }
        public string HrefLang { get; set; }
        public string GoogleTagManager { get; set; }
        public string GoogleAnalytics { get; set; }

        public string GoogleSiteVerification { get; set; }

        public string BingSiteVerification { get; set; }
        public string SchemaIds { get; set; }
        public List<CustomSchemaItem> SchemaItems { get; set; }

    }
    public class SchemaItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int CustomEntityId { get; set; }
       

    }
    public class CustomSchemaItem
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }

}
