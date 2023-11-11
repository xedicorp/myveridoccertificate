﻿using Cofoundry.Domain.Data;

namespace Cofoundry.Domain.Internal;

public class GetPageVersionBlockEntityMicroSummariesByIdRangeQueryHandler
    : IQueryHandler<GetPageVersionBlockEntityMicroSummariesByIdRangeQuery, IDictionary<int, RootEntityMicroSummary>>
    , IPermissionRestrictedQueryHandler<GetPageVersionBlockEntityMicroSummariesByIdRangeQuery, IDictionary<int, RootEntityMicroSummary>>
{
    private readonly CofoundryDbContext _dbContext;
    private readonly IEntityDefinitionRepository _entityDefinitionRepository;

    public GetPageVersionBlockEntityMicroSummariesByIdRangeQueryHandler(
        CofoundryDbContext dbContext,
        IEntityDefinitionRepository entityDefinitionRepository
        )
    {
        _dbContext = dbContext;
        _entityDefinitionRepository = entityDefinitionRepository;
    }

    public async Task<IDictionary<int, RootEntityMicroSummary>> ExecuteAsync(GetPageVersionBlockEntityMicroSummariesByIdRangeQuery query, IExecutionContext executionContext)
    {
        var results = await Query(query).ToDictionaryAsync(e => e.ChildEntityId, e => (RootEntityMicroSummary)e);

        return results;
    }

    private IQueryable<ChildEntityMicroSummary> Query(GetPageVersionBlockEntityMicroSummariesByIdRangeQuery query)
    {
        var definition = _entityDefinitionRepository.GetRequiredByCode(PageEntityDefinition.DefinitionCode);

        var dbQuery = _dbContext
            .PageVersionBlocks
            .AsNoTracking()
            .FilterActive()
            .Where(m => query.PageVersionBlockIds.Contains(m.PageVersionBlockId))
            .Select(m => new ChildEntityMicroSummary()
            {
                ChildEntityId = m.PageVersionBlockId,
                RootEntityId = m.PageVersion.PageId,
                RootEntityTitle = m.PageVersion.Title,
                EntityDefinitionCode = definition.EntityDefinitionCode,
                EntityDefinitionName = definition.Name,
                IsPreviousVersion = !m.PageVersion.PagePublishStatusQueries.Any()
            });

        return dbQuery;
    }


    public IEnumerable<IPermissionApplication> GetPermissions(GetPageVersionBlockEntityMicroSummariesByIdRangeQuery query)
    {
        yield return new PageReadPermission();
    }
}
