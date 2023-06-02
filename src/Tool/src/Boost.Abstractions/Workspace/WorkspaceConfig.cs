using System;
using System.Collections.Generic;

namespace Boost.Workspace;

public class WorkspaceConfig
{
    public IEnumerable<WorkspaceSuperBoost> SuperBoosts { get; set; }
     = Array.Empty<WorkspaceSuperBoost>();

    public IEnumerable<WebLink> WebLinks { get; set; } = Array.Empty<WebLink>();
}
