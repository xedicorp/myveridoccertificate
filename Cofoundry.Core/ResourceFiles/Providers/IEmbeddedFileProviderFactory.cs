﻿using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Cofoundry.Core.ResourceFiles;

/// <summary>
/// Factory for abstrating away the creation of IFileProvider
/// instances for embedded resources. Used so we can have an 
/// enhanced file provider for embedded resources.
/// </summary>
public interface IEmbeddedFileProviderFactory
{
    IFileProvider Create(Assembly assembly);
}
