using System;
using System.Collections.Generic;

namespace Boost.Workspace;

public class WorkspaceSuperBoost
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public IEnumerable<SuperBoostAction> Actions { get; set; }
        = Array.Empty<SuperBoostAction>();
}
