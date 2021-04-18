namespace Boost.Snapshooter
{
    public class SnapshotInfo
    {
        public string Directory { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public string? MissmatchFileName { get; set; }

        public bool HasMismatch { get; set; }
    }
}
