using Octokit;

namespace Boost.GitHub;

public record GitHubConnectionContext(
    GitHubConnectedService Service,
    GitHubClient Client);
