using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace Boost.AzureDevOps
{
    public interface IAzureDevOpsGitClient
    {
        Task<IEnumerable<GitCommitRef>> GetLastCommitsAsync(
            Guid serviceId,
            Guid repositoryId,
            string branch,
            int top = 10,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<GitRepository>> GetAllAsync(
            Guid serviceId,
            CancellationToken cancellationToken);

        Task<GitRepository> GetByIdAsync(
            Guid serviceId,
            Guid id,
            CancellationToken cancellationToken);

        Task<byte[]?> GetFileContentAsync(
            Guid serviceId,
            string id,
            string path,
            CancellationToken cancellationToken);
        Task<GitRepository> GetByNameAsync(Guid serviceId, string name, CancellationToken cancellationToken);
    }
}
