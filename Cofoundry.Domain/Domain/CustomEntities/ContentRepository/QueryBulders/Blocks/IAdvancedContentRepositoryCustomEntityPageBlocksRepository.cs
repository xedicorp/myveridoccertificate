﻿namespace Cofoundry.Domain;

/// <summary>
/// Queries and commands for page block data on custom entity pages.
/// </summary>
public interface IAdvancedContentRepositoryCustomEntityPageBlocksRepository
{
    /// <summary>
    /// Query for page blocks in custom entity pages by an id.
    /// </summary>
    /// <param name="customEntityVersionPageBlockId">Database id of the custom entity version page block to get.</param>
    IAdvancedContentRepositoryCustomEntityPageBlockByIdQueryBuilder GetById(int customEntityVersionPageBlockId);

    /// <summary>
    /// Adds a new block to a template region on a custom entity page.
    /// </summary>
    /// <param name="command">Command parameters.</param>
    /// <returns>Id of the newly created block.</returns>
    Task<int> AddAsync(AddCustomEntityVersionPageBlockCommand command);

    /// <summary>
    /// Updates an existing block within a template region 
    /// of a custom entity page.
    /// </summary>
    /// <param name="command">Command parameters.</param>
    Task UpdateAsync(UpdateCustomEntityVersionPageBlockCommand command);

    /// <summary>
    /// Updates an existing block within a template region 
    /// of a custom entity page.
    /// </summary>
    /// <param name="customEntityVersionPageBlockId">
    /// Id of the block to update.
    /// </param>
    /// <param name="commandPatcher">
    /// An action to configure or "patch" a command that's been initialized
    /// with the existing block data.
    /// </param>
    Task UpdateAsync(int customEntityVersionPageBlockId, Action<UpdateCustomEntityVersionPageBlockCommand> commandPatcher);

    /// <summary>
    /// Moves a block up or down within a multi-block region 
    /// on a custom entity page.
    /// </summary>
    /// <param name="command">Command parameters.</param>
    Task MoveAsync(MoveCustomEntityVersionPageBlockCommand command);

    /// <summary>
    /// Deletes a block from a template region on a custom entity page.
    /// </summary>
    /// <param name="customEntityVersionPageBlockId">Id of the block to delete.</param>
    Task DeleteAsync(int customEntityVersionPageBlockId);
}
