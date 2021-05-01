using System;
using System.Collections.Generic;
using System.Linq;
using Boost.Git;
using Boost.Settings;

namespace Boost.GitHub
{
    public class GitHubConnectedServiceProvider : IConnectedServiceProvider
    {
        public ConnectedServiceType Type => new ConnectedServiceType(
            GitHubConstants.ServiceTypeName,
            new ConnectedServiceFeature[] { ConnectedServiceFeature.GitRemoteRepository }
            )
        {
            SecretProperties = new string[] { "PersonalAccessToken", "AccessToken", "OAuth.Secret" }
        };

        public IConnectedService MapService(ConnectedService service)
        {
            var gitHubService = new GitHubConnectedService(service.Id, service.Name)
            {
                DefaultWorkRoot = service.DefaultWorkRoot,
                Owner = service.GetPropertyValue("Owner")
            };

            var mode = service.GetPropertyValue("Mode");

            if (mode == "OAuth")
            {
                gitHubService.OAuth = new GitHubOAuthConfig(
                    service.GetPropertyValue("OAuth.ClientId"),
                    service.GetPropertyValue("OAuth.Secret"));

                gitHubService.AccessToken = service.TryGetPropertyValue<string>("AccessToken");

            }
            else
            {
                gitHubService.AccessToken = service.GetPropertyValue("PersonalAccessToken");
            }

            return gitHubService;
        }

        public IConnectedService? MatchServiceFromGitRemoteReference(IGitRemoteReference remoteReference, IEnumerable<IConnectedService> connectedServices)
        {
            if (remoteReference is GitHubRemoteReference gitHubRef)
            {
                IEnumerable<GitHubConnectedService> gitHubServices = connectedServices
                    .Where(x => x is GitHubConnectedService a)
                    .Select(x => x as GitHubConnectedService)!;

                GitHubConnectedService? ownerMatche = gitHubServices
                    .Where(x => x.Owner is { })
                    .FirstOrDefault(x => x.Owner!.Equals(
                        gitHubRef.Owner,
                        StringComparison.InvariantCultureIgnoreCase));

                return ownerMatche ?? gitHubServices.FirstOrDefault();
            }

            return null;
        }

        public IGitRemoteReference? ParseRemoteUrl(IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                if (url.Contains("github.com"))
                {
                    var uri = new Uri(url);
                    var parts = uri.LocalPath.Split(
                        '/',
                        StringSplitOptions.RemoveEmptyEntries);

                    return new GitHubRemoteReference(
                        url,
                        parts[1].Replace(".git", ""),
                        parts[0]);
                }
            }

            return null;
        }
    }

    public record GitHubRemoteReference(string Url, string Name, string Owner)
    : IGitRemoteReference
    { }
}
