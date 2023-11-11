namespace Cofoundry.Web.Admin;

public class SeoToolsRouteLibrary : AngularModuleRouteLibrary
{
    public const string RoutePrefix = "seotools";

    public SeoToolsRouteLibrary(AdminSettings adminSettings)
        : base(adminSettings, RoutePrefix, RouteConstants.InternalModuleResourcePathPrefix)
    {
    }

    public string List()
    {
        return AngularRoute();
    }

    public string New()
    {
        return AngularRoute("new");
    }

    public string Details(int id)
    {
        return AngularRoute(id.ToString());
    }
}