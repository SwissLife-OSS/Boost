namespace Boost.Nuget;

public record NugetPackageInfo(string PackageId)
{
    public NugetPackageVersionInfo? LatestStable { get; init; }
    public NugetPackageVersionInfo? LatestPreRelease { get; init; }
}
