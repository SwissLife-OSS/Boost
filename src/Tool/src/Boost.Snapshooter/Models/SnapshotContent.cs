namespace Boost.Snapshooter
{
    public record SnapshotContent(string Name, string Snapshot)
    {
        public string? Mismatch { get; init; }
    }
}
