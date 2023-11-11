﻿using Cofoundry.Domain.Data;

namespace Cofoundry.Domain.Internal;

public class GetCustomEntityVersionEntityMicroSummariesByIdRangeQueryHandler
    : IQueryHandler<GetCustomEntityVersionEntityMicroSummariesByIdRangeQuery, IDictionary<int, RootEntityMicroSummary>>
    , IIgnorePermissionCheckHandler
{
    private readonly CofoundryDbContext _dbContext;
    private readonly IPermissionValidationService _permissionValidationService;

    public GetCustomEntityVersionEntityMicroSummariesByIdRangeQueryHandler(
        CofoundryDbContext dbContext,
        IPermissionValidationService permissionValidationService
        )
    {
        _dbContext = dbContext;
        _permissionValidationService = permissionValidationService;
    }

    public async Task<IDictionary<int, RootEntityMicroSummary>> ExecuteAsync(GetCustomEntityVersionEntityMicroSummariesByIdRangeQuery query, IExecutionContext executionContext)
    {
        var results = await Query(query).ToDictionaryAsync(e => e.ChildEntityId, e => (RootEntityMicroSummary)e);
        EnforcePermissions(results, executionContext);

        return results;
    }

    private IQueryable<ChildEntityMicroSummary> Query(GetCustomEntityVersionEntityMicroSummariesByIdRangeQuery query)
    {
        var dbQuery = _dbContext
            .CustomEntityVersions
            .AsNoTracking()
            .Where(v => query.CustomEntityVersionIds.Contains(v.CustomEntityVersionId))
            .Select(v => new ChildEntityMicroSummary()
            {
                ChildEntityId = v.CustomEntityVersionId,
                RootEntityId = v.CustomEntityId,
                RootEntityTitle = v.Title,
                EntityDefinitionName = v.CustomEntity.CustomEntityDefinition.EntityDefinition.Name,
                EntityDefinitionCode = v.CustomEntity.CustomEntityDefinition.EntityDefinition.EntityDefinitionCode,
                IsPreviousVersion = !v.CustomEntityPublishStatusQueries.Any()
            });

        return dbQuery;
    }

    private void EnforcePermissions(IDictionary<int, RootEntityMicroSummary> entities, IExecutionContext executionContext)
    {
        var definitionCodes = entities.Select(e => e.Value.EntityDefinitionCode);

        _permissionValidationService.EnforceCustomEntityPermission<CustomEntityReadPermission>(definitionCodes, executionContext.UserContext);
    }
}