using Microsoft.Data.SqlClient;
namespace VeriDocCertificate.CofoundaryCMS;

public class FeatureCategorySummary
{
    public int CategoryId { get; set; }

    public string Title { get; set; }

    public string ShortDescription { get; set; }
    public int SortOrder { get; set; }

}