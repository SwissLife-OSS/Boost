using System.Collections.Generic;

namespace Boost.Workspace;

public class WorkspaceContext
{
    public string CurrentDirectory { get; set; }

    public IEnumerable<FileSystemItem> Files { get; set; }
}


public record GitBranch(string Name);

public record RemoteRepositoryReference(
    string Url,
    string Name,
    RemoteRepositorySource Source)
{
    public string? Account { get; init; }

    public string? Project { get; init; }
}


public record GitTag(string Name);

public record GitHead(string Branch)
{
    public int? AheadBy { get; init; }
    public int? BehindBy { get; init; }
    public string? Message { get; init; }
    public string? Sha { get; init; }
}

public enum RemoteRepositorySource
{
    AzureDevOps,
    GitHub
}
