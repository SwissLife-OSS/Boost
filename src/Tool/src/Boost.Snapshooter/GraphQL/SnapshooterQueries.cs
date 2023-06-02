using System.Collections.Generic;
using Boost.Snapshooter;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.GraphQL;

[ExtendObjectType(Name = RootTypes.Query)]
public class SnapshooterQueries
{
    public IEnumerable<SnapshotInfo> GetSnapshooterSnaps(
        bool withMismatchOnly,
        [Service] ISnapshooterService snapshooterService)
    {
        return snapshooterService.GetSnapshots(withMismatchOnly);
    }

    public IEnumerable<SnapshotDirectory> GetSnapshooterDirectories(
        bool withMismatchOnly,
        [Service] ISnapshooterService snapshooterService)
    {
        return snapshooterService.GetDirectories(withMismatchOnly);
    }

    public SnapshotContent GetSnapshooterSnapshot(
        GetSnapshooterSnapshotInput input,
        [Service] ISnapshooterService snapshooterService)
    {
        return snapshooterService.GetSnapshot(input.FileName, input.MissmatchFileName);
    }
}

public record GetSnapshooterSnapshotInput(string FileName, string? MissmatchFileName);

public record ApproveSnapshotInput(string FileName, string? MissmatchFileName);
