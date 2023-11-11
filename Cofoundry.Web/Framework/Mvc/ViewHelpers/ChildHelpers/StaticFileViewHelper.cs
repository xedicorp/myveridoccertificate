﻿using Cofoundry.Core.Web;

namespace Cofoundry.Web;

/// <summary>
/// Helper for resolving urls to static files.
/// </summary>
public class StaticFileViewHelper : IStaticFileViewHelper
{
    private readonly IStaticFilePathFormatter _staticFilePathFormatter;
    private readonly ISiteUrlResolver _siteUriResolver;

    public StaticFileViewHelper(
        IStaticFilePathFormatter staticFilePathFormatter,
        ISiteUrlResolver siteUriResolver
        )
    {
        _staticFilePathFormatter = staticFilePathFormatter;
        _siteUriResolver = siteUriResolver;
    }

    /// <summary>
    /// Appends a version hash querystring parameter to the end
    /// of the file path, e.g. 'myfile.js?v=examplecomputedhash'
    /// </summary>
    /// <param name="applicationRelativePath">
    /// The static resource file path, which must be the full application 
    /// relative path.
    /// </param>
    /// <returns>
    /// If the file is found, the path is returned with the version 
    /// appended, otherwise the unmodified path is returned.
    /// </returns>
    public string AppendVersion(string applicationRelativePath)
    {
        var appended = _staticFilePathFormatter.AppendVersion(applicationRelativePath);
        return appended;
    }

    /// <summary>
    /// Maps a relative static file url to an absolute one with a version parameter.
    /// </summary>
    /// <param name="applicationRelativePath">
    /// The static resource file path, which must be the full application 
    /// relative path.
    /// </param>
    /// <returns>
    /// If the file is found, the path is resolved with the version 
    /// appended, otherwise the resolved path is returned.
    /// </returns>
    public string ToAbsoluteWithFileVersion(string applicationRelativePath)
    {
        if (string.IsNullOrWhiteSpace(applicationRelativePath)) return applicationRelativePath;
        var appenedUrl = AppendVersion(applicationRelativePath);
        var resolvedUrl = _siteUriResolver.MakeAbsolute(appenedUrl);

        return resolvedUrl;
    }
}
