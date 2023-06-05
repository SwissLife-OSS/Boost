using Semver;

namespace Boost.Nuget;

public static class NugetPackageExtensions
{
    public static SemVersion ToSemVersion(this NugetPackageVersionInfo versionInfo)
    {
        return SemVersion.Parse(versionInfo.Version);
    }
}
