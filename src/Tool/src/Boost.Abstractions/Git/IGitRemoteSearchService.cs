using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Git;

public interface IGitRemoteSearchService
{
    Task<IEnumerable<GitRemoteRepository>> SearchAsync(
        SearchGitRepositoryRequest request,
        CancellationToken cancellationToken);
}

public record SearchGitRepositoryRequest(string Term)
{
    public IEnumerable<string>? Services { get; init; }

}
