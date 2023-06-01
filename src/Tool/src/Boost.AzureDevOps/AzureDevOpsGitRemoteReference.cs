using Boost.Git;

namespace Boost.AzureDevOps;

public record AzureDevOpsGitRemoteReference(string Url, string Name, string Account)
    : IGitRemoteReference
{
    public string? Project { get; init; }
}
