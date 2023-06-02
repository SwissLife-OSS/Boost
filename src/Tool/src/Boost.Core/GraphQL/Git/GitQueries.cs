using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Git;
using Boost.GraphQL;
using Boost.Infrastructure;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.Core.GraphQL;

[ExtendObjectType(RootTypes.Query)]
public partial class GitQueries
{
    public async Task<IEnumerable<GitRemoteRepository>> SearchRepositoriesAsync(
        [Service] IGitRemoteSearchService searchService,
        SearchRepositoriesInput input,
        CancellationToken cancellationToken)
    {
        return await searchService.SearchAsync(
            new SearchGitRepositoryRequest(input.Term),
            cancellationToken);
    }

    public IEnumerable<GitLocalRepository> SearchLocalRepositories(
        [Service] IGitLocalRepositoryService gitService,
        SearchLocalRepositoriesInput input)
    {
        return gitService.Search(input.Term);
    }

    public Task<GitRemoteRepository> GetRemoteGitRepositoryAsync(
        [Service] IGitRemoteService gitService,
        GetRemoteGitRepositoryInput input,
        CancellationToken cancellationToken)
    {
        return gitService.GetAsync(input.ServiceId, input.Id, cancellationToken);
    }

    public async Task<GitLocalRepository?> GetLocalGitRepository(
        [Service] IGitLocalRepositoryService gitService,
        [Service] IBoostApplicationContext applicationContext,
        string? id,
        CancellationToken cancellationToken)
    {
        var path = applicationContext.WorkingDirectory.FullName;

        if (id is { })
        {
            path = RepositoryUtils.DecodeId(id);
        }

        return await gitService.GetRepositoryAsync(path, cancellationToken);
    }
}
