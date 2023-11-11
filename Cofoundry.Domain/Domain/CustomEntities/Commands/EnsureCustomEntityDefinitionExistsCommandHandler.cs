﻿using Cofoundry.Domain.Data;

namespace Cofoundry.Domain.Internal;

/// <summary>
/// CustomEntityDefinitions are definied in code and stored in the database, so if they are missing
/// from the databse we need to add them. Execute this to ensure that the custom entity definition has been saved
/// to the database before assigning it to another entity.
/// </summary>
public class EnsureCustomEntityDefinitionExistsCommandHandler
    : ICommandHandler<EnsureCustomEntityDefinitionExistsCommand>
    , IIgnorePermissionCheckHandler
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly CofoundryDbContext _dbContext;
    private readonly ICustomEntityDefinitionRepository _customEntityDefinitionRepository;

    public EnsureCustomEntityDefinitionExistsCommandHandler(
        ICommandExecutor commandExecutor,
        CofoundryDbContext dbContext,
        ICustomEntityDefinitionRepository customEntityDefinitionRepository
        )
    {
        _commandExecutor = commandExecutor;
        _dbContext = dbContext;
        _customEntityDefinitionRepository = customEntityDefinitionRepository;
    }

    public async Task ExecuteAsync(EnsureCustomEntityDefinitionExistsCommand command, IExecutionContext executionContext)
    {
        var customEntityDefinition = _customEntityDefinitionRepository.GetRequiredByCode(command.CustomEntityDefinitionCode);
        var dbDefinition = await _dbContext
            .CustomEntityDefinitions
            .FilterByCode(command.CustomEntityDefinitionCode)
            .SingleOrDefaultAsync();

        if (dbDefinition == null)
        {
            await _commandExecutor.ExecuteAsync(new EnsureEntityDefinitionExistsCommand(command.CustomEntityDefinitionCode), executionContext);

            dbDefinition = new CustomEntityDefinition()
            {
                CustomEntityDefinitionCode = customEntityDefinition.CustomEntityDefinitionCode,
                ForceUrlSlugUniqueness = customEntityDefinition.ForceUrlSlugUniqueness,
                HasLocale = customEntityDefinition.HasLocale
            };

            if (customEntityDefinition is IOrderableCustomEntityDefinition)
            {
                dbDefinition.IsOrderable = ((IOrderableCustomEntityDefinition)customEntityDefinition).Ordering != CustomEntityOrdering.None;
            }

            _dbContext.CustomEntityDefinitions.Add(dbDefinition);
            await _dbContext.SaveChangesAsync();
        }
    }
}