using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.Data;
using Boost.Settings;
using LibGit2Sharp;
using Serilog;

namespace Boost.Git;

public class LocalRepositoryIndexer : ILocalRepositoryIndexer
{
    private readonly IBoostDbContextFactory _dbContextFactory;
    private readonly IConnectedServiceManager _connectedServiceManager;

    public LocalRepositoryIndexer(
        IBoostDbContextFactory dbContextFactory,
        IConnectedServiceManager connectedServiceManager)
    {
        _dbContextFactory = dbContextFactory;
        _connectedServiceManager = connectedServiceManager;
    }

    public async Task<int> IndexWorkRootAsync(
        WorkRoot workRoot,
        Action<string>? onProgress = null,
        CancellationToken cancellationToken = default)
    {
        using IBoostDbContext dbContext = _dbContextFactory.Open(DbOpenMode.ReadWrite);

        IEnumerable<IConnectedService> connectedServices = await _connectedServiceManager
            .GetServicesAsync(cancellationToken);

        ClearIndex(workRoot.Name);
        int indexCount = 0;

        foreach (DirectoryInfo? dir in new DirectoryInfo(workRoot.Path)
            .GetDirectories())
        {
            AddToIndex(workRoot, dir.FullName, connectedServices, dbContext);

            GitRepositoryIndex? repoIndex = Index(dir.FullName);
            onProgress?.Invoke(dir.FullName);
            indexCount++;
        }

        return indexCount;
    }

    public async Task IndexRepository(
        WorkRoot workRoot,
        string path,
        CancellationToken cancellationToken)
    {
        using IBoostDbContext dbContext = _dbContextFactory.Open(DbOpenMode.ReadWrite);

        IEnumerable<IConnectedService> connectedServices = await _connectedServiceManager
            .GetServicesAsync(cancellationToken);

        AddToIndex(workRoot, path, connectedServices, dbContext);
    }

    private void AddToIndex(
        WorkRoot workRoot,
        string path,
        IEnumerable<IConnectedService> connectedServices,
        IBoostDbContext dbContext)
    {
        Log.Information("Indexing: {Path}", path);

        GitRepositoryIndex? repoIndex = Index(path);

        if (repoIndex is { })
        {
            repoIndex.WorkRoot = workRoot.Name;

            IConnectedService? connectedService = _connectedServiceManager
                .MatchServiceFromGitRemote(repoIndex.RemoteReference, connectedServices);

            if (connectedService is { })
            {
                repoIndex.ServiceId = connectedService.Id;
            }

            dbContext.GitRepos.Insert(repoIndex);
        }
    }

    private int ClearIndex(string workRoot)
    {
        using IBoostDbContext dbContext = _dbContextFactory.Open(DbOpenMode.ReadWrite);

        return dbContext.GitRepos.DeleteMany(x => x.WorkRoot == workRoot);
    }

    private GitRepositoryIndex? Index(string directory)
    {
        Repository? repo = RepositoryUtils.TryGetRepository(directory);

        if (repo is { })
        {
            var repoIndex = new GitRepositoryIndex
            {
                Id = RepositoryUtils.EncodeId(directory),
                Name = new DirectoryInfo(directory).Name,
                WorkingDirectory = repo.Info.WorkingDirectory,
            };

            GitLocalCommit? lastCommit = GetLastCommit(repo);

            if (lastCommit is { })
            {
                repoIndex.LastCommitDate = lastCommit.CreatedAt;
            }

            if (repo.Network is { } n && n.Remotes is { } r && r.Any())
            {
                repoIndex.RemoteReference = _connectedServiceManager
                    .GetGitRemoteReference(r.Select(x => x.Url));
            }

            return repoIndex;
        }

        return null;
    }

    private GitLocalCommit? GetLastCommit(Repository repo)
    {
        Commit? commit = repo.Commits
            .OrderByDescending(x => x.Committer.When)
            .FirstOrDefault();

        if (commit is { })
        {
            return new GitLocalCommit(commit.Sha, commit.Message, commit.Committer.When);
        }

        return null;
    }
}
