﻿namespace Cofoundry.Domain.Data;

/// <summary>
/// Abstraction of data required for an entity versioning table e.g. for
/// PageVersion or CustomEntityVersion.
/// </summary>
public interface IEntityVersion
{
    int WorkFlowStatusId { get; set; }

    DateTime CreateDate { get; set; }
}
