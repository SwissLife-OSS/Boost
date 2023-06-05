using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.Settings;

namespace Boost.Git;

public class GitRemoteService : IGitRemoteService
{
    private readonly IGitRemoteClientFactory _clientFactory;
    private readonly IConnectedServiceManager _serviceManager;

    public GitRemoteService(
        IGitRemoteClientFactory clientFactory,
        IConnectedServiceManager serviceManager)
    {
        _clientFactory = clientFactory;
        _serviceManager = serviceManager;
    }

    public async Task<GitRemoteRepository> GetAsync(
        Guid serviceId,
        string id,
        CancellationToken cancellationToken)
    {
        IGitRemoteClient client = await CreateClient(serviceId, cancellationToken);

        return await client.GetAsync(serviceId, id, cancellationToken);
    }

    public async Task<GitRemoteRepository?> GetByRemoteReferenceAsync(
        Guid serviceId,
        IGitRemoteReference reference,
        string name,
        CancellationToken cancellationToken)
    {
        IGitRemoteClient client = await CreateClient(serviceId, cancellationToken);

        return await client.GetByRemoteReference(
            serviceId,
            reference,
            name,
            cancellationToken);
    }

    public async Task<IEnumerable<GitRemoteCommit>> GetLastCommitsAsync(
        Guid serviceId,
        string id,
        int top,
        CancellationToken cancellationToken)
    {
        IGitRemoteClient client = await CreateClient(serviceId, cancellationToken);

        return await client.GetLastCommitsAsync(
            serviceId,
            id,
            top,
            cancellationToken);
    }

    public async Task<byte[]?> GetFileContentAsync(
        Guid serviceId,
        string id,
        string path,
        CancellationToken cancellationToken)
    {
        IGitRemoteClient client = await CreateClient(serviceId, cancellationToken);

        return await client.GetFileContentAsync(serviceId, id, path, cancellationToken);
    }

    private async Task<IGitRemoteClient> CreateClient(
        Guid serviceId,
        CancellationToken cancellationToken)
    {
        IConnectedService? service = await _serviceManager
            .GetAsync(serviceId, cancellationToken);

        if (service is null)
        {
            throw new ApplicationException($"No service found for Id: {_serviceManager}");
        }

        return _clientFactory.Create(service.Type);
    }
}
