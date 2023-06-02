using Boost.Snapshooter;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.GraphQL;

[ExtendObjectType(Name = RootTypes.Mutation)]
public partial class SnapshooterMutations
{
    public ApproveSnapshotPayload ApproveSnapshot(
        ApproveSnapshotInput input,
        [Service] ISnapshooterService snapshooterService)
    {
        SnapshotContent? snap = snapshooterService
            .ApproveSnapshot(input.FileName, input.MissmatchFileName);

        return new ApproveSnapshotPayload(snap);
    }

    public int ApproveAllMismatches(
        [Service] ISnapshooterService snapshooterService)
    {
        int count = snapshooterService
             .ApproveAllMismatches();

        return count;
    }
}
