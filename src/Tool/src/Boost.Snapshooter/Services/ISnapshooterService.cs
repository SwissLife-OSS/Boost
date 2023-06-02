using System.Collections.Generic;

namespace Boost.Snapshooter;

public interface ISnapshooterService
{
    int ApproveAllMismatches();
    SnapshotContent ApproveSnapshot(string fileName, string? missmatchFilename);
    IEnumerable<SnapshotDirectory> GetDirectories(bool withMismatchOnly);
    SnapshotContent GetSnapshot(string fileName, string? missmatchFilename);
    IEnumerable<SnapshotInfo> GetSnapshots(bool withMismatchOnly);
}
