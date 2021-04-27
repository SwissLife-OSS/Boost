using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Versioning;

namespace Boost.Nuget
{
    public interface INugetService
    {
        Task<NugetPackageVersionInfo> GetLatestVersionAsync(
            string sourceName,
            string packageId,
            CancellationToken cancellationToken);
        Task<NugetPackageVersionInfo> GetLatestVersionAsync(string packageId, CancellationToken cancellationToken);

        Task<NugetPackageInfo> GetNugetPackageInfo(
            string? sourceName,
            string packageId,
            CancellationToken cancellationToken);
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
