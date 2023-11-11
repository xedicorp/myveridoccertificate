﻿namespace Cofoundry.Domain;

/// <summary>
/// IAdvancedContentRespository extension root for the ImageAsset entity.
/// </summary>
public interface IAdvancedContentRepositoryImageAssetRepository
{
    /// <summary>
    /// Retrieve an image asset by a unique database id.
    /// </summary>
    /// <param name="imageAssetId">ImageAssetId of the image to get.</param>
    IAdvancedContentRepositoryImageAssetByIdQueryBuilder GetById(int imageAssetId);

    /// <summary>
    /// Retrieve a set of image assets using a batch of database ids.
    /// The Cofoundry.Core dictionary extensions can be useful for 
    /// ordering the results e.g. results.FilterAndOrderByKeys(ids).
    /// </summary>
    /// <param name="imageAssetIds">Range of ImageAssetIds of the images to get.</param>
    IContentRepositoryImageAssetByIdRangeQueryBuilder GetByIdRange(IEnumerable<int> imageAssetIds);

    /// <summary>
    /// Search for image assets, returning paged lists of data.
    /// </summary>
    IContentRepositoryImageAssetSearchQueryBuilder Search();

    /// <summary>
    /// Adds a new image asset.
    /// </summary>
    /// <param name="command">Command parameters.</param>
    /// <returns>Id of the newly created user.</returns>
    Task<int> AddAsync(AddImageAssetCommand command);

    /// <summary>
    /// Updates the properties of an existing image asset. Updating
    /// the file is optional, but if you do then existing links to the
    /// asset file will redirect to the new asset file.
    /// </summary>
    /// <param name="command">Command parameters.</param>
    Task UpdateAsync(UpdateImageAssetCommand command);

    /// <summary>
    /// Updates the properties of an existing image asset. Updating
    /// the file is optional, but if you do then existing links to the
    /// asset file will redirect to the new asset file.
    /// </summary>
    /// <param name="imageAssetId">
    /// Database id of the image asset to update.
    /// </param>
    /// <param name="commandPatcher">
    /// An action to configure or "patch" a command that's been initialized
    /// with the existing image asset data.
    /// </param>
    Task UpdateAsync(int imageAssetId, Action<UpdateImageAssetCommand> commandPatcher);

    /// <summary>
    /// Removes an image asset from the system and
    /// queues any related files or caches to be removed
    /// as a separate process.
    /// </summary>
    /// <param name="imageAssetId">Id of the image asset to delete.</param>
    Task DeleteAsync(int imageAssetId);
}
