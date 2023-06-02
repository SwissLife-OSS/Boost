using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Settings;

namespace Boost.Git;

public interface IGitRemoteService
{
    Task<GitRemoteRepository> GetAsync(
        Guid serviceId,
        string id,
        CancellationToken cancellationToken);
    Task<GitRemoteRepository?> GetByRemoteReferenceAsync(Guid serviceId, IGitRemoteReference reference, string name, CancellationToken cancellationToken);
    Task<byte[]?> GetFileContentAsync(
        Guid serviceId,
        string id,
        string path,
        CancellationToken cancellationToken);

    Task<IEnumerable<GitRemoteCommit>> GetLastCommitsAsync(
        Guid serviceId,
        string id,
        int top,
        CancellationToken cancellationToken);
}
