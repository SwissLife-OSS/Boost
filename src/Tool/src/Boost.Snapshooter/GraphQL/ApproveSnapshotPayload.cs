

using Boost.Snapshooter;

namespace Boost.GraphQL
{
    public partial class SnapshooterMutations
    {
        public class ApproveSnapshotPayload
        {
            public ApproveSnapshotPayload(SnapshotContent snapshotContent)
            {
                Snapshot = snapshotContent;
            }

            public SnapshotContent Snapshot { get; }
        }
    }
}
