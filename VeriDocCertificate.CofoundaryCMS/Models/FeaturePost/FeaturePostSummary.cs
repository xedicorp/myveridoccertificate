namespace VeriDocCertificate.CofoundaryCMS;

public class FeaturePostSummary
{
    public string Title { get; set; }

    public int Categorylist { get; set; }

    public string ShortDescription { get; set; }

    public DocumentAssetRenderDetails ThumbnailIcon { get; set; }

    public string MeidaLookup { get; set; }

    public string VideoLookup { get; set; }

    public DocumentAssetRenderDetails ThumbnailImageAsset { get; set; }

    public string ThumbnailIframeId { get; set; }

    public string FullPath { get; set; }

    public DateTime? PostDate { get; set; }
    public int SortOrder { get; set; }
}