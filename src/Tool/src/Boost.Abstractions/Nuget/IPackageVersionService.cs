using System;
using System.Collections.Generic;

namespace Boost.Nuget
{
    public interface IPackageVersionService
    {
        IEnumerable<PackageVersion> GetVersions(string fileName);
        Dictionary<string, IEnumerable<PackageVersion>> GetVersionsByDirectory(string? root);
    }

    public record PackageVersion(string Name, string Version)
    {
        public string? Reference { get; init; }
    }

    public record NugetPackageVersionInfo(string Version, DateTimeOffset Published);

    public record NugetPackageInfo(string PackageId)
    {
        public NugetPackageVersionInfo? LatestStable { get; init; }
        public NugetPackageVersionInfo? LatestPreRelease { get; init; }
    }
}
