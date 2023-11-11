namespace Cofoundry.Web.Admin;

public class HeaderFooterModuleRegistration : IInternalAngularModuleRegistration
{
    private readonly IAdminRouteLibrary _adminRouteLibrary;
    private readonly PagesSettings _pagesSettings;

    public HeaderFooterModuleRegistration(
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
            AdminModuleCode = "COFCHF",
            Title = "Custom Header Footer",
            Description = "Manage the directories in your site.",
            MenuCategory = AdminModuleMenuCategory.ManageSite,
            PrimaryOrdering = AdminModuleMenuPrimaryOrdering.Secondry,
            Url = _adminRouteLibrary.HeaderFooter.List(),
            RestrictedToPermission = new PageDirectoryAdminModulePermission()
        };

        return module;
    }

    public string RoutePrefix
    {
        get { return HeaderFooterRouteLibrary.RoutePrefix; }
    }
}
