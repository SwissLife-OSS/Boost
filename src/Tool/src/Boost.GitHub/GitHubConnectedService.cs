using System;
using Boost.Settings;

namespace Boost.GitHub;

public class GitHubConnectedService : IConnectedService
{
    public Guid Id { get; set; }

    public string Type => "GitHub";

    public GitHubConnectedService(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Name { get; set; }

    public string? Owner { get; set; }

    public GitHubOAuthConfig? OAuth { get; set; }

    public string? AccessToken { get; set; }

    public string? DefaultWorkRoot { get; set; }
}
