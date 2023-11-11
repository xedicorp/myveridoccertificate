namespace Cofoundry.Domain.Data;

[Obsolete("The document asset grouping system will be revised in an upcomming release.")]
public class DocumentAssetGroup : ICreateAuditable
{
    public DocumentAssetGroup()
    {
        DocumentAssetGroupItems = new List<DocumentAssetGroupItem>();
        ChildDocumentAssetGroups = new List<DocumentAssetGroup>();
    }

    public int DocumentAssetGroupId { get; set; }
    public string GroupName { get; set; }
    public Nullable<int> ParentDocumentAssetGroupId { get; set; }
    public virtual ICollection<DocumentAssetGroupItem> DocumentAssetGroupItems { get; set; }
    public virtual ICollection<DocumentAssetGroup> ChildDocumentAssetGroups { get; set; }
    public virtual DocumentAssetGroup ParentDocumentAssetGroup { get; set; }

    public System.DateTime CreateDate { get; set; }
    public int CreatorId { get; set; }
    public virtual User Creator { get; set; }
}
