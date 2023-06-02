using System;
using System.Collections.Generic;
using System.Linq;
using Boost.Git;
using Boost.Settings;

namespace Boost.AzureDevOps;

public class AzureDevOpsConnectedServiceProvider : IConnectedServiceProvider
{
    public ConnectedServiceType Type => new ConnectedServiceType(
        AzureDevOpsConstants.ServiceTypeName,
        new ConnectedServiceFeature[]
        {
            ConnectedServiceFeature.GitRemoteRepository,
            ConnectedServiceFeature.Pipelines
        })
    {
        SecretProperties = new string[] { "PersonalAccessToken" }
    };

    public IConnectedService MapService(ConnectedService service)
    {
        return new AzureDevOpsConnectedService(
            service.Id,
            service.Name,
            service.GetPropertyValue("Account"),
            service.GetPropertyValue("DefaultProject"),
            service.GetPropertyValue("PersonalAccessToken"))
        {
            DefaultWorkRoot = service.DefaultWorkRoot
        };
    }

    public IConnectedService? MatchServiceFromGitRemoteReference(
        IGitRemoteReference remoteReference,
        IEnumerable<IConnectedService> connectedServices)
    {
        if (remoteReference is AzureDevOpsGitRemoteReference adoRef)
        {
            IEnumerable<AzureDevOpsConnectedService> adoServices = connectedServices
                .Where(x => x is AzureDevOpsConnectedService a)
                .Select(x => x as AzureDevOpsConnectedService)!;

            IEnumerable<AzureDevOpsConnectedService> accountMaches = adoServices
                .Where(x => x.Account.Equals(
                    adoRef.Account,
                    StringComparison.InvariantCultureIgnoreCase));

            AzureDevOpsConnectedService? projectMatch = accountMaches
                .FirstOrDefault(x => x.DefaultProject.Equals(
                    adoRef.Project,
                    StringComparison.InvariantCultureIgnoreCase));

            return projectMatch ?? accountMaches.FirstOrDefault();
        }

        return null;
    }

    public IGitRemoteReference? ParseRemoteUrl(IEnumerable<string> urls)
    {
        foreach (var url in urls)
        {
            if (url.Contains("dev.azure.com"))
            {
                var uri = new Uri(url);
                var parts = uri.LocalPath.Split(
                    '/',
                    StringSplitOptions.RemoveEmptyEntries);

                return new AzureDevOpsGitRemoteReference(
                    url,
                    parts.Last(),
                    parts[0])
                {
                    Project = parts[1]
                };
            }
            else if (url.Contains(".visualstudio.com"))
            {
                var uri = new Uri(url);
                var authoritParts = uri.Authority.Split('.');
                var parts = uri.LocalPath.Split(
                    '/',
                    StringSplitOptions.RemoveEmptyEntries);

                return new AzureDevOpsGitRemoteReference(
                    url,
                    parts.Last(),
                    authoritParts[0])
                {
                    Project = parts[parts.Length - 3]
                };
            }
        }

        return null;
    }
}
