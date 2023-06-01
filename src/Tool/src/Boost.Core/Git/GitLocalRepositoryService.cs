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

public class GitLocalRepositoryService : IGitLocalRepositoryService
{
    private readonly IBoostDbContextFactory _dbContextFactory;
    private readonly IConnectedServiceManager _connectedServiceManager;

    public GitLocalRepositoryService(
        IBoostDbContextFactory dbContextFactory,
        IConnectedServiceManager connectedServiceManager)
    {
        _dbContextFactory = dbContextFactory;
        _connectedServiceManager = connectedServiceManager;
    }

    public IEnumerable<GitLocalRepository> Search(
        string? term)
    {
        IEnumerable<GitRepositoryIndex> repos;
        using IBoostDbContext dbContext = _dbContextFactory.Open(DbOpenMode.ReadOnly);

        if (term is { })
        {
            repos = dbContext.GitRepos
                .Find(x => x.Name.ToLower().Contains(term.ToLower()));
        }
        else
        {
            repos = dbContext.GitRepos.FindAll();
        }

        return repos.Select(ToLocalGitRepository);
    }

    public int GetLocalRepositoryCount()
    {
        using IBoostDbContext dbContext = _dbContextFactory.Open(DbOpenMode.ReadOnly);
        return dbContext.GitRepos.Count();
    }

    public async Task<GitLocalRepository?> GetRepositoryAsync(
        string path,
        CancellationToken cancellationToken)
    {
        Repository? repo = RepositoryUtils.TryGetRepository(path);

        if (repo is { })
        {
            var localRepo = new GitLocalRepository
            {
                Id = RepositoryUtils.EncodeId(path),
                Name = new DirectoryInfo(path).Name,
                WorkingDirectory = repo.Info.WorkingDirectory,
                Head = GetHead(repo.Head),
                Tags = repo.Tags.Select(x => new GitTag(x.FriendlyName)),
                Commits = GetCommits(repo).ToArray(),
                Branches = repo.Branches.Where(x => !x.IsRemote)
                    .Select(b => new GitBranch(b.FriendlyName))
                    .ToArray(),
                RemoteReference = GetRemoteReference(repo)
            };

            if (localRepo.RemoteReference is { })
            {
                IEnumerable<IConnectedService> services = await _connectedServiceManager
                    .GetServicesAsync(cancellationToken);

                IConnectedService? remoteService = _connectedServiceManager
                    .MatchServiceFromGitRemote(
                        localRepo.RemoteReference,
                        services);

                localRepo.RemoteServiceId = remoteService?.Id;
            }

            return localRepo;
        }

        return null;
    }

    private IGitRemoteReference? GetRemoteReference(Repository repo)
    {
        if (repo.Network is { } n && n.Remotes is { } r && r.Any())
        {
            return _connectedServiceManager
                .GetGitRemoteReference(r.Select(x => x.Url));
        }

        return null;
    }

    public async Task<byte[]?> GetFileContent(
        string id,
        string path,
        CancellationToken cancellationToken)
    {
        var root = RepositoryUtils.DecodeId(id);

        var filename = Path.Combine(root, path);

        if (File.Exists(filename))
        {
            return await File.ReadAllBytesAsync(filename, cancellationToken);
        }

        return null;
    }

    private IEnumerable<GitLocalCommit> GetCommits(Repository repo)
    {
        return repo.Commits
            .Select(x => new GitLocalCommit(
               x.Sha,
               x.Message,
               x.Committer.When)
            {
                Author = x.Committer.Name
            });
    }

    private GitHead GetHead(Branch head)
    {
        return new GitHead(head.FriendlyName)
        {
            AheadBy = head.TrackingDetails.AheadBy,
            BehindBy = head.TrackingDetails.BehindBy,
            Message = head.TrackingDetails?.CommonAncestor?.Message,
            Sha = head.TrackingDetails?.CommonAncestor?.Sha,
        };
    }

    public Repository? TryGetRepository(string path)
    {
        try
        {
            var repo = new Repository(path);

            return repo;
        }
        catch (RepositoryNotFoundException)
        {
            return null;
        }
        catch
        {
            throw;
        }
    }

    public async Task<GitLocalRepository?> GetByRemoteAsync(
        Guid serviceId,
        string name,
        CancellationToken cancellationToken)
    {
        try
        {
            using IBoostDbContext dbContext = _dbContextFactory.Open(DbOpenMode.ReadOnly);
            GitRepositoryIndex? repo = dbContext.GitRepos.FindOne(x =>
                x.ServiceId == serviceId &&
                x.Name.ToLower() == name.ToLower());

            if (repo is { })
            {
                return ToLocalGitRepository(repo);
            }
        }
        catch (Exception ex)
        {
            Log.Warning(
                ex, "" +
                "Could not load local repository from remote. {ServiceId} {Name}",
                serviceId,
                name);
        }

        return null;
    }

    private GitLocalRepository ToLocalGitRepository(GitRepositoryIndex repo)
    {
        return new GitLocalRepository
        {
            Id = repo.Id,
            Name = repo.Name,
            RemoteServiceId = repo.ServiceId,
            WorkingDirectory = repo.WorkingDirectory,
            WorkRoot = repo.WorkRoot
        };
    }
}
