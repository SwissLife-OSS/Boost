using System.Collections.Generic;

namespace Boost.Snapshooter
{
    public record SnapshotDirectory(
        string Name,
        string FullName,
        IEnumerable<SnapshotInfo> Snapshots);
}
