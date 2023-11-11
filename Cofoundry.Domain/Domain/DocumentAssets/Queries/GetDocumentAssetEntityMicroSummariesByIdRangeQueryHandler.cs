﻿using Cofoundry.Domain.Data;

namespace Cofoundry.Domain.Internal;

public class GetDocumentAssetEntityMicroSummariesByIdRangeQueryHandler
    : IQueryHandler<GetDocumentAssetEntityMicroSummariesByIdRangeQuery, IDictionary<int, RootEntityMicroSummary>>
    , IPermissionRestrictedQueryHandler<GetDocumentAssetEntityMicroSummariesByIdRangeQuery, IDictionary<int, RootEntityMicroSummary>>
{
    private readonly CofoundryDbContext _dbContext;
    private readonly IEntityDefinitionRepository _entityDefinitionRepository;

    public GetDocumentAssetEntityMicroSummariesByIdRangeQueryHandler(
        CofoundryDbContext dbContext,
        IEntityDefinitionRepository entityDefinitionRepository
        )
    {
        _dbContext = dbContext;
        _entityDefinitionRepository = entityDefinitionRepository;
    }

    public async Task<IDictionary<int, RootEntityMicroSummary>> ExecuteAsync(GetDocumentAssetEntityMicroSummariesByIdRangeQuery query, IExecutionContext executionContext)
    {
        var results = await Query(query).ToDictionaryAsync(e => e.RootEntityId);

        return results;
    }

    private IQueryable<RootEntityMicroSummary> Query(GetDocumentAssetEntityMicroSummariesByIdRangeQuery query)
    {
        var definition = _entityDefinitionRepository.GetRequiredByCode(DocumentAssetEntityDefinition.DefinitionCode);

        var dbQuery = _dbContext
            .DocumentAssets
            .AsNoTracking()
            .FilterByIds(query.DocumentAssetIds)
            .Select(a => new RootEntityMicroSummary()
            {
                RootEntityId = a.DocumentAssetId,
                RootEntityTitle = a.Title,
                EntityDefinitionCode = definition.EntityDefinitionCode,
                EntityDefinitionName = definition.Name
            });

        return dbQuery;
    }

    public IEnumerable<IPermissionApplication> GetPermissions(GetDocumentAssetEntityMicroSummariesByIdRangeQuery query)
    {
        yield return new DocumentAssetReadPermission();
    }
}
