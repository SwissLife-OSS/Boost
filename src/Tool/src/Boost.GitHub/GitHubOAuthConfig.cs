namespace Boost.GitHub;

public class GitHubOAuthConfig
{
    public GitHubOAuthConfig(string clientId, string secret)
    {
        Secret = secret;
        ClientId = clientId;
    }

    public GitHubOAuthConfig()
    {
        //for Config only
    }

    public string ClientId { get; set; } = default!;

    public string Secret { get; set; } = default!;
}
