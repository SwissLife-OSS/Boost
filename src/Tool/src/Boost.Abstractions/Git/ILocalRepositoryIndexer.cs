using System;
using System.Threading;
using System.Threading.Tasks;
using Boost.Settings;

namespace Boost.Git
{
    public interface ILocalRepositoryIndexer
    {
        Task<int> IndexWorkRootAsync(
            WorkRoot workRoot,
            Action<string>? onProgress = null,
            CancellationToken cancellationToken = default);
    }
}
