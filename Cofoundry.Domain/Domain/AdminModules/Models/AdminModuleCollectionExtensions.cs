﻿namespace Cofoundry.Domain;

public static class AdminModuleCollectionExtensions
{
    public static IEnumerable<AdminModule> SetStandardOrdering(this IEnumerable<AdminModule> source)
    {
        return source
            .OrderBy(r => r.MenuCategory)
            .ThenBy(r => r.PrimaryOrdering)
            .ThenByDescending(r => r.SecondaryOrdering)
            .ThenBy(r => r.Title);
    }
}
