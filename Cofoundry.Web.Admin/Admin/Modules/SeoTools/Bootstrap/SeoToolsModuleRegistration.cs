namespace Cofoundry.Web.Admin;

public class SeoToolsModuleRegistration : IInternalAngularModuleRegistration
{
    private readonly IAdminRouteLibrary _adminRouteLibrary;
    private readonly PagesSettings _pagesSettings;

    public SeoToolsModuleRegistration(
        IAdminRouteLibrary adminRouteLibrary,
        PagesSettings pagesSettings
        )
    {
        _adminRouteLibrary = adminRouteLibrary;
        _pagesSettings = pagesSettings;
    }

    public AdminModule GetModule()
    {
        if (_pagesSettings.Disabled) return null;

        var module = new AdminModule()
        {
            AdminModuleCode = "XSEOTL",
            Title = "SEO Tools",
            Description = "Manage your site SEO.",
            MenuCategory = AdminModuleMenuCategory.ManageSite,
            PrimaryOrdering = AdminModuleMenuPrimaryOrdering.Secondry,
            Url = _adminRouteLibrary.SeoTools.List(),
            RestrictedToPermission = new PageDirectoryAdminModulePermission()
        };

        return module;
    }

    public string RoutePrefix
    {
        get { return SeoToolsRouteLibrary.RoutePrefix; }
    }
}
