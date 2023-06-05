using System.Threading;
using System.Threading.Tasks;

namespace Boost.Nuget;

public interface INugetService
{
    Task<NugetPackageInfo?> GetNugetPackageInfoAsync(string packageId, CancellationToken cancellationToken);
}
