using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Versioning;

namespace Boost.Nuget
{
    public interface INugetService
    {
        Task<NugetPackageInfo?> GetNugetPackageInfoAsync(string packageId, CancellationToken cancellationToken);
    }
}
