using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Git;
using Octokit;
using Serilog;

namespace Boost.GitHub;

public class GitHubRepoClient : IGitRemoteClient
{
    private readonly GitHubClientFactory _clientFactory;

    public GitHubRepoClient(GitHubClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public string ConnectedServiceType => GitHubConstants.ServiceTypeName;

    public async Task<IEnumerable<GitRemoteRepository>> SearchAsync(
        Guid serviceId,
        string searchString,
        CancellationToken cancellationToken)
    {
        GitHubConnectedService service = await _clientFactory.ConnectAsync(serviceId, cancellationToken);

        GitHubClient client = _clientFactory.CreateClient(serviceId);
        var request = new SearchRepositoriesRequest(searchString);
        request.User = service.Owner;

        SearchRepositoryResult searchResult = await client.Search.SearchRepo(request);

        return searchResult.Items.Select(x => ToRemoteRepository(service, x));
    }

    public async Task<Repository> GetByIdAsync(
        Guid serviceId,
        long id,
        CancellationToken cancellationToken)
    {
        await _clientFactory.ConnectAsync(
            serviceId,
            cancellationToken);

        GitHubClient client = _clientFactory.CreateClient(serviceId);
        Repository repo = await client.Repository.Get(id);

        return repo;
    }

    public async Task<GitRemoteRepository> GetAsync(
        Guid serviceId,
        string id,
        CancellationToken cancellationToken)
    {
        Repository repo = await GetByIdAsync(serviceId, long.Parse(id), cancellationToken);

        return ToRemoteRepository(_clientFactory.Connections[serviceId].Service, repo);
    }

    public async Task<GitRemoteRepository?> GetByRemoteReference(
        Guid serviceId,
        IGitRemoteReference reference,
        string name,
        CancellationToken cancellationToken)
    {
        if (reference is GitHubRemoteReference ghRef && ghRef.Owner is string)
        {
            await _clientFactory.ConnectAsync(
                serviceId,
                cancellationToken);

            GitHubClient client = _clientFactory.CreateClient(serviceId);

            try
            {
                Repository? repo = await client.Repository.Get(ghRef.Owner, name);

                if (repo is { })
                {
                    return ToRemoteRepository(_clientFactory.Connections[serviceId].Service, repo);
                }
            }
            catch (NotFoundException ex)
            {
                Log.Warning(ex, "GitHub repository not found: {Owner} {Name}", ghRef.Owner, name);
            }
        }

        return null;
    }

    public async Task<IEnumerable<GitRemoteCommit>> GetLastCommitsAsync(
        Guid serviceId,
        string id,
        int top,
        CancellationToken cancellationToken)
    {
        GitHubClient client = _clientFactory.CreateClient(serviceId);

        IReadOnlyList<GitHubCommit>? commits = await client.Repository.Commit
            .GetAll(long.Parse(id), new ApiOptions { PageSize = top });

        return commits.Select(ToRemoteCommit);
    }

    public async Task<byte[]?> GetFileContentAsync(
        Guid serviceId,
        string id,
        string path,
        CancellationToken cancellationToken)
    {
        GitHubClient client = _clientFactory.CreateClient(serviceId);
        Repository repo = await GetByIdAsync(serviceId, long.Parse(id), cancellationToken);

        try
        {
            byte[]? data = await client.Repository.Content.GetRawContent(
                repo.Owner.Login,
                repo.Name,
                path);

            return data;
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Error in loading content in {RepoId}, {Path}", id, path);
        }

        return null;
    }

    private GitRemoteRepository ToRemoteRepository(
        GitHubConnectedService service, Repository repo)
    {
        return new GitRemoteRepository
        {
            Id = repo.Id.ToString(),
            ServiceId = service.Id,
            Name = repo.Name,
            FullName = repo.FullName,
            Source = service.Name,
            Url = repo.Url,
            WebUrl = repo.HtmlUrl
        };
    }

    private GitRemoteCommit ToRemoteCommit(GitHubCommit commit)
    {
        return new GitRemoteCommit(
                commit.Sha,
                commit.Commit.Message,
                commit.Commit.Committer.Date.UtcDateTime,
                commit.Author?.Login)
        {
            WebUrl = commit.HtmlUrl
        };
    }
}
