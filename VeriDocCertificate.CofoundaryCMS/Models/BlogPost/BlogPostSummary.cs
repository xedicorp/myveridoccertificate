namespace VeriDocCertificate.CofoundaryCMS;

public class BlogPostSummary
{
    public string Title { get; set; }

    public string ShortDescription { get; set; }
    public List<int> Categorylist  { get; set; }

    public DocumentAssetRenderDetails ThumbnailImageAsset { get; set; }

    public string FullPath { get; set; }

    public DateTime? PostDate { get; set; }
    public int SortOrder { get; set; }
}