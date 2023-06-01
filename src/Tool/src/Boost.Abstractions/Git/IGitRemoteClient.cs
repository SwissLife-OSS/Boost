using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Git;

public interface IGitRemoteClient
{
    string ConnectedServiceType { get; }

    Task<GitRemoteRepository> GetAsync(
        Guid serviceId,
        string id,
        CancellationToken cancellationToken);
    Task<GitRemoteRepository?> GetByRemoteReference(Guid serviceId, IGitRemoteReference reference, string name, CancellationToken cancellationToken);
    Task<byte[]?> GetFileContentAsync(Guid serviceId, string id, string path, CancellationToken cancellationToken);
    Task<IEnumerable<GitRemoteCommit>> GetLastCommitsAsync(
        Guid serviceId,
        string id,
        int top,
        CancellationToken cancellationToken);

    Task<IEnumerable<GitRemoteRepository>> SearchAsync(
        Guid id,
        string searchString,
        CancellationToken cancellationToken);
}
