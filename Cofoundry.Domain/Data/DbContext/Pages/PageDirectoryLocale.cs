namespace Cofoundry.Domain.Data;

public class PageDirectoryLocale : ICreateAuditable
{
    public int PageDirectoryLocaleId { get; set; }
    public int PageDirectoryId { get; set; }
    public int LocaleId { get; set; }
    public string UrlPath { get; set; }
    public virtual Locale Locale { get; set; }
    public virtual PageDirectory PageDirectory { get; set; }

    public System.DateTime CreateDate { get; set; }
    public int CreatorId { get; set; }
    public virtual User Creator { get; set; }
}
