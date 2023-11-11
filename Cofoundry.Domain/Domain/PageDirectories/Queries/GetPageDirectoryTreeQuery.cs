﻿namespace Cofoundry.Domain;

/// <summary>
/// Query to return a complete tree of page directory nodes, starting
/// with the root directory as a single node.
/// </summary>
public class GetPageDirectoryTreeQuery : IQuery<PageDirectoryNode>
{
}
