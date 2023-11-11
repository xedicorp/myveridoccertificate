﻿namespace Cofoundry.Web.Admin;

public class AdminMenuViewModel
{
    public ICollection<AdminModule> ManageSiteModules { get; set; }

    public ICollection<AdminModule> SettingsModules { get; set; }

    public AdminModuleMenuCategory SelectedCategory { get; set; }

    public AdminModule SelectedModule { get; set; }
}
