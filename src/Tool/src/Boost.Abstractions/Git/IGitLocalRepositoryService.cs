using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Git
{
    public interface IGitLocalRepositoryService
    {
        Task<GitLocalRepository?> GetByRemoteAsync(Guid serviceId, string name, CancellationToken cancellationToken);
        Task<byte[]?> GetFileContent(string id, string path, CancellationToken cancellationToken);
        Task<GitLocalRepository?> GetRepositoryAsync(
            string path,
            CancellationToken cancellationToken);

        IEnumerable<GitLocalRepository> Search(string term);
    }
}
