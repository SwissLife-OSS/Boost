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

namespace Boost.Git
{
    public class LocalRepositoryIndexer : ILocalRepositoryIndexer
    {
        private readonly IBoostDbContext _liteDbContext;
        private readonly IConnectedServiceManager _connectedServiceManager;

        public LocalRepositoryIndexer(
            IBoostDbContext dbContext,
            IConnectedServiceManager connectedServiceManager)
        {
            _liteDbContext = dbContext;
            _connectedServiceManager = connectedServiceManager;
        }

        public async Task<int> IndexWorkRootAsync(
            WorkRoot workRoot,
            Action<string>? onProgress = null,
            CancellationToken cancellationToken = default)
        {
            IEnumerable<IConnectedService> connectedServices = await _connectedServiceManager
                .GetServicesAsync(cancellationToken);

            ClearIndex(workRoot.Name);
            int indexCount = 0;

            foreach (DirectoryInfo? dir in new DirectoryInfo(workRoot.Path)
                .GetDirectories())
            {
                Log.Information("Indexing: {Path}", dir.FullName);

                GitRepositoryIndex? repoIndex = Index(dir.FullName);

                if (repoIndex is { })
                {
                    repoIndex.WorkRoot = workRoot.Name;
                    onProgress?.Invoke(dir.FullName);

                    IConnectedService? connectedService = _connectedServiceManager
                        .MatchServiceFromGitRemote(repoIndex.RemoteReference, connectedServices);

                    if (connectedService is { })
                    {
                        repoIndex.ServiceId = connectedService.Id;
                    }

                    _liteDbContext.GitRepos.Insert(repoIndex);
                    indexCount++;
                }
            }

            return indexCount;
        }

        private int ClearIndex(string workRoot)
        {
            return _liteDbContext.GitRepos.DeleteMany(x => x.WorkRoot == workRoot);
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
}
