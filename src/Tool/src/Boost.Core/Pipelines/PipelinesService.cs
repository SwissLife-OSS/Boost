using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.Settings;

namespace Boost.Pipelines
{
    public class PipelinesService : IPipelinesService
    {
        private readonly IConnectedServiceManager _serviceManager;
        private readonly IEnumerable<IPipelinesClient> _pipelinesClients;

        public PipelinesService(
            IConnectedServiceManager serviceManager,
            IEnumerable<IPipelinesClient> pipelinesClients)
        {
            _serviceManager = serviceManager;
            _pipelinesClients = pipelinesClients;
        }

        public async Task<IEnumerable<Pipeline>> GetPipelinesAsync(
            Guid serviceId,
            string repositoryId,
            CancellationToken cancellationToken)
        {
            IPipelinesClient? client = await CreateClientAsync(serviceId, cancellationToken);

            if (client is { })
            {
                return await client.GetPipelinesAsync(serviceId, repositoryId, cancellationToken);
            }

            return Array.Empty<Pipeline>();
        }

        public async Task<IEnumerable<PipelineRun>> GetRunsAsync(
            Pipeline pipeline,
            int top,
            CancellationToken cancellationToken)
        {
            IPipelinesClient? client = await CreateClientAsync(pipeline.ServiceId, cancellationToken);

            if ( client is { })
            {
                return await client.GetRunsAsync(pipeline, top, cancellationToken);
            }

            return Array.Empty<PipelineRun>();
        }

        private async Task<IPipelinesClient?> CreateClientAsync(
            Guid serviceId,
            CancellationToken cancellationToken)
        {
            IConnectedService? service = await _serviceManager
                .GetAsync(serviceId, cancellationToken);

            if (service is { })
            {
                return CreateClient(service.Type);
            }

            return null;
        }


        private IPipelinesClient? CreateClient(string serviceType)
        {
            IPipelinesClient? client = _pipelinesClients
                .SingleOrDefault(x => x.ConnectedServiceType == serviceType);

            return client;
        }
    }
}
