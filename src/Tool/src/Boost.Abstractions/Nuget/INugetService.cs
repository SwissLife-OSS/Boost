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

        Task<NugetPackageInfo> GetNugetPackageInfo(
            string? sourceName,
            string packageId,
            CancellationToken cancellationToken);
    }
}
