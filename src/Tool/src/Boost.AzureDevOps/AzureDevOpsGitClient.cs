using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Git;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;

namespace Boost.AzureDevOps;

public class AzureDevOpsGitClient : AzureDevOpsClient,
    IAzureDevOpsGitClient,
    IGitRemoteClient
{
    public string ConnectedServiceType => AzureDevOpsConstants.ServiceTypeName;

    public AzureDevOpsGitClient(
        AzureDevOpsClientFactory azureDevOpsClientFactory)
        : base(azureDevOpsClientFactory)
    { }

    public async Task<IEnumerable<GitRepository>> GetAllAsync(
            Guid serviceId,
            CancellationToken cancellationToken)
    {
        AzureDevOpsConnectedService service = await ClientFactory.ConnectAsync(
            serviceId,
            cancellationToken);

        GitHttpClient client = ClientFactory
            .CreateClient<GitHttpClient>(serviceId);

        return await client.GetRepositoriesAsync(
            service.DefaultProject,
            cancellationToken: cancellationToken);
    }

    public async Task<GitRepository> GetByIdAsync(
        Guid serviceId,
        Guid id,
        CancellationToken cancellationToken)
    {
        await ClientFactory.ConnectAsync(
            serviceId,
            cancellationToken);

        GitHttpClient client = ClientFactory.CreateClient<GitHttpClient>(serviceId);

        return await client.GetRepositoryAsync(
                repositoryId: id,
                cancellationToken);
    }

    public async Task<GitRepository> GetByNameAsync(
        Guid serviceId,
        string name,
        CancellationToken cancellationToken)
    {
        await ClientFactory.ConnectAsync(
            serviceId,
            cancellationToken);

        GitHttpClient client = ClientFactory.CreateClient<GitHttpClient>(serviceId);

        return await client.GetRepositoryAsync(
                repositoryId: name,
                cancellationToken);
    }

    public async Task<byte[]?> GetFileContentAsync(
        Guid serviceId,
        string id,
        string path,
        CancellationToken cancellationToken)
    {
        GitHttpClient client = ClientFactory.CreateClient<GitHttpClient>(serviceId);

        try
        {
            Stream stream = await client
                .GetItemContentAsync(id, path, cancellationToken: cancellationToken);

            using MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);

            return ms.ToArray();
        }
        catch (VssServiceException nfe) when (nfe.Message.StartsWith("TF401174"))
        {
            return null;
        }
    }

    public async Task<IEnumerable<GitCommitRef>> GetLastCommitsAsync(
        Guid serviceId,
        Guid repositoryId,
        string branch,
        int top = 10,
        CancellationToken cancellationToken = default)
    {
        AzureDevOpsConnectedService service = await ClientFactory.ConnectAsync(
            serviceId,
            cancellationToken);

        GitHttpClient client = ClientFactory.CreateClient<GitHttpClient>(serviceId);

        return await client.GetCommitsAsync(
                service.DefaultProject,
                repositoryId,
                new GitQueryCommitsCriteria()
                {
                    ItemVersion = new GitVersionDescriptor()
                    {
                        VersionType = GitVersionType.Branch,
                        VersionOptions = GitVersionOptions.None,
                        Version = branch
                    },
                    IncludeLinks = true
                },
                top: top,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<GitRemoteCommit>> GetLastCommitsAsync(
        Guid serviceId,
        string id,
        int top,
        CancellationToken cancellationToken)
    {
        IEnumerable<GitCommitRef>? commits = await GetLastCommitsAsync(
            serviceId,
            Guid.Parse(id),
            branch: null,
            top: top,
            cancellationToken);

        return commits.Select(ToRemoteCommit);
    }

    public async Task<GitRemoteRepository> GetAsync(
        Guid serviceId,
        string id,
        CancellationToken cancellationToken)
    {
        GitRepository repo = await GetByIdAsync(serviceId, Guid.Parse(id), cancellationToken);

        return ToGitRemoteRepository(ClientFactory.Connections[serviceId].Service, repo);
    }

    public async Task<GitRemoteRepository?> GetByRemoteReference(
        Guid serviceId,
        IGitRemoteReference reference,
        string name,
        CancellationToken cancellationToken)
    {
        if (reference is AzureDevOpsGitRemoteReference adoRef)
        {
            AzureDevOpsConnectedService service = await ClientFactory.ConnectAsync(
                serviceId,
                cancellationToken);

            GitHttpClient client = ClientFactory.CreateClient<GitHttpClient>(serviceId);

            GitRepository? repo = await client.GetRepositoryAsync(
                adoRef.Project,
                name,
                cancellationToken: cancellationToken);

            if (repo is { })
            {
                return ToGitRemoteRepository(service, repo);
            }
        }

        return null;
    }

    public async Task<IEnumerable<GitRemoteRepository>> SearchAsync(
        Guid id,
        string searchString,
        CancellationToken cancellationToken)
    {
        IEnumerable<GitRepository>? repos = await GetAllAsync(id, cancellationToken);

        return repos
            .Where(x => x.Name.Contains(
                searchString,
                StringComparison.InvariantCultureIgnoreCase))
            .Select(x => ToGitRemoteRepository(ClientFactory.Connections[id].Service, x));
    }

    private GitRemoteRepository ToGitRemoteRepository(
        AzureDevOpsConnectedService service,
        GitRepository repo)
    {
        return new GitRemoteRepository
        {
            Id = repo.Id.ToString("N"),
            ServiceId = service.Id,
            Source = service.Type,
            Name = repo.Name,
            FullName = $"{repo.ProjectReference.Name}/{repo.Name}",
            WebUrl = repo.WebUrl,
            Url = repo.RemoteUrl
        };
    }

    private GitRemoteCommit ToRemoteCommit(GitCommitRef x)
    {
        return new GitRemoteCommit(
                x.CommitId,
                x.Comment,
                x.Committer.Date,
                x.Committer.Name)
        {
            WebUrl = x.Links.GetLink("web")
        };
    }
}
