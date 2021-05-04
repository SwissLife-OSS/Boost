using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Boost.Infrastructure;
using Newtonsoft.Json;

namespace Boost.Snapshooter
{
    public class SnapshooterService : ISnapshooterService
    {
        private readonly IBoostApplicationContext _boostApplicationContext;

        public SnapshooterService(IBoostApplicationContext boostApplicationContext)
        {
            _boostApplicationContext = boostApplicationContext;
        }

        public IEnumerable<SnapshotInfo> GetSnapshots(bool withMismatchOnly)
        {
            List<SnapshotInfo> snapshots = new();
            List<FileInfo> mismatches = new();

            foreach (FileInfo? ss in _boostApplicationContext.WorkingDirectory
                .GetFiles("*.snap", SearchOption.AllDirectories))
            {
                if (ss.Directory.Name.Contains("__snapshots__"))
                {
                    snapshots.Add(new SnapshotInfo
                    {
                        Name = ss.Name,
                        FileName = ss.FullName,
                        Directory = ss.Directory.FullName
                    });
                }
                else if (ss.Directory.Name.Contains("__mismatch__"))
                {
                    mismatches.Add(ss);
                }
            }

            foreach (FileInfo? mm in mismatches)
            {
                SnapshotInfo? refSs = snapshots
                    .FirstOrDefault(x => x.Name == mm.Name);

                if (refSs is { })
                {
                    refSs.MissmatchFileName = mm.FullName;
                    refSs.HasMismatch = true;
                }
            }

            if (withMismatchOnly)
            {
                snapshots = snapshots.Where(x => x.HasMismatch).ToList();
            }

            return snapshots;
        }

        public IEnumerable<SnapshotDirectory> GetDirectories(bool withMismatchOnly)
        {
            IEnumerable<SnapshotInfo>? snapshots = GetSnapshots(withMismatchOnly);

            foreach (IGrouping<string, SnapshotInfo>? group in
                snapshots.GroupBy(x => x.Directory))
            {
                yield return new SnapshotDirectory(
                    GetDirectoryName(group.Key), group.Key,
                    group.ToList());
            }
        }

        public SnapshotContent GetSnapshot(string fileName, string? missmatchFilename)
        {
            string? missMatchContent = null;
            if (!string.IsNullOrEmpty(missmatchFilename))
            {
                missMatchContent = GetJson(missmatchFilename);
            }

            return new SnapshotContent(
                Path.GetFileName(fileName),
                GetJson(fileName))
            {
                Mismatch = missMatchContent
            };
        }

        public SnapshotContent ApproveSnapshot(string fileName, string? missmatchFilename)
        {
            var file = new FileInfo(missmatchFilename);

            if (file.Exists)
            {
                File.Move(missmatchFilename, fileName, overwrite: true);

                if (!file.Directory.EnumerateFiles().Any())
                {
                    Directory.Delete(file.Directory.FullName);
                }
            }

            return GetSnapshot(fileName, missmatchFilename: null);
        }

        public int ApproveAllMismatches()
        {
            IEnumerable<SnapshotInfo>? snaps = GetSnapshots(true);

            foreach (SnapshotInfo? snap in snaps)
            {
                var file = new FileInfo(snap.MissmatchFileName!);

                File.Move(
                    snap.MissmatchFileName!,
                    snap.FileName,
                    overwrite: true);

                if (!file.Directory.EnumerateFiles().Any())
                {
                    Directory.Delete(file.Directory.FullName);
                }
            }

            return snaps.Count();
        }

        private string GetJson(string fileName)
        {
            var json = File.ReadAllText(fileName);

            try
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(json);

                return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }
            catch
            {
                return json;
            }
        }

        private string GetDirectoryName(string directory)
        {
            var pars = directory.Split(
                Path.DirectorySeparatorChar,
                StringSplitOptions.RemoveEmptyEntries);

            return pars[pars.Length - 2];
        }
    }
}
