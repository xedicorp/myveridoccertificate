namespace VeriDocCertificate.CofoundaryCMS;

/// <summary>
/// Inheriting from SimplePageableQuery or IPageableQuery 
/// gives us a few extra features when working with pages 
/// data via the PagingExtensions set of extension methods.
/// 
/// See https://www.cofoundry.org/docs/framework/data-access/paged-queries.cs
/// </summary>
public class SearchFeaturePostsQuery : SimplePageableQuery
{
    public int CategoryId { get; set; }
}