using System;
using System.Threading;
using System.Threading.Tasks;
using Boost.Settings;

namespace Boost.Git;

public interface ILocalRepositoryIndexer
{
    Task IndexRepository(WorkRoot workRoot, string path, CancellationToken cancellationToken);
    Task<int> IndexWorkRootAsync(
        WorkRoot workRoot,
        Action<string>? onProgress = null,
        CancellationToken cancellationToken = default);
}
