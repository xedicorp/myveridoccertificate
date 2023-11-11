﻿namespace Cofoundry.Domain;

public sealed class PageVersionEntityDefinition : IDependableEntityDefinition
{
    public const string DefinitionCode = "COFPGV";

    public string EntityDefinitionCode => DefinitionCode;

    public string Name => "Page Version";

    public IQuery<IDictionary<int, RootEntityMicroSummary>> CreateGetEntityMicroSummariesByIdRangeQuery(IEnumerable<int> ids)
    {
        return new GetPageVersionEntityMicroSummariesByIdRangeQuery(ids);
    }
}
