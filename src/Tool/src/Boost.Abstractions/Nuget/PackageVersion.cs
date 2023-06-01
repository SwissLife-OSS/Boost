namespace Boost.Nuget;

public record PackageVersion(string Name, string Version)
{
    public string? Reference { get; init; }
}
