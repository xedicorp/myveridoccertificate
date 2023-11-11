﻿using Cofoundry.Domain.Data;

namespace Cofoundry.Domain.Internal;

/// <summary>
/// Simple mapper for mapping to ImageAssetDetails objects.
/// </summary>
public interface IImageAssetDetailsMapper
{
    /// <summary>
    /// Maps an EF ImageAsset record from the db into a ImageAssetDetails 
    /// object. If the db record is null then null is returned.
    /// </summary>
    /// <param name="dbImage">ImageAsset record from the database.</param>
    ImageAssetDetails Map(ImageAsset dbImage);
}
