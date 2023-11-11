namespace VeriDocCertificate.CofoundaryCMS.Models
{
    public class FeatureListVM
    {
       

        public PagedQueryResult<FeaturePostSummary> FeaturePostModel { get; set; }

        public ICollection<FeatureCategorySummary> FeatureCategories { get; set; }

    }
}
