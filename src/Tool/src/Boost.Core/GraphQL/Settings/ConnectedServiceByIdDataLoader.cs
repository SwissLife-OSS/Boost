using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.Settings;
using GreenDonut;

namespace Boost.Core.GraphQL;

public class ConnectedServiceByIdDataLoader : BatchDataLoader<Guid, ConnectedService>
{
    private readonly IConnectedServiceManager _connectedServiceManager;

    public ConnectedServiceByIdDataLoader(
        IConnectedServiceManager connectedServiceManager,
        IBatchScheduler batchScheduler)
        : base(batchScheduler)
    {
        _connectedServiceManager = connectedServiceManager;
    }

    protected override async Task<IReadOnlyDictionary<Guid, ConnectedService>> LoadBatchAsync(
        IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken)
    {
        IEnumerable<IConnectedService> services = await _connectedServiceManager
            .GetServicesAsync(cancellationToken);

        return services.Select(x => new ConnectedService
        {
            Id = x.Id,
            Name = x.Name,
            Type = x.Type,
        }).ToDictionary(x => x.Id);
    }
}
