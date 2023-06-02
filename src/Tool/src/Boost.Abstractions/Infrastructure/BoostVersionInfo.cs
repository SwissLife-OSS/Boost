using Boost.Nuget;

namespace Boost.Infrastructure;

public class BoostVersionInfo
{
    public string PackageId { get; set; }

    public string? Installed { get; set; }

    public NugetPackageVersionInfo? Latest { get; set; }

    public NugetPackageVersionInfo? PreRelease { get; set; }
    public bool NewerAvailable { get; set; }
    public bool NewerPreReleaseAvailable { get; set; }
}
