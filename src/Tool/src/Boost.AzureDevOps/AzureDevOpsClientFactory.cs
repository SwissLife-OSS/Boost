using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.Settings;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace Boost.AzureDevOps
{
    public record AzureDevOpsConnectionContext(
        AzureDevOpsConnectedService Service,
        VssConnection Connection);

    public class AzureDevOpsClientFactory
    {
        private readonly IConnectedServiceManager _connectedServiceManager;
        private readonly Dictionary<Guid, AzureDevOpsConnectionContext> _connections
            = new Dictionary<Guid, AzureDevOpsConnectionContext>();

        public AzureDevOpsClientFactory(
            IConnectedServiceManager connectedServiceManager)
        {
            _connectedServiceManager = connectedServiceManager;
        }

        internal Dictionary<Guid, AzureDevOpsConnectionContext> Connections => _connections;

        public TClient CreateClient<TClient>(
                Guid id)
            where TClient : VssHttpClientBase
        {
            VssConnection connection = _connections[id].Connection;

            return connection.GetClient<TClient>();
        }

        public async Task<AzureDevOpsConnectedService> ConnectAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            if (!_connections.ContainsKey(id))
            {
                AzureDevOpsConnectedService service = await GetConnectedServiceAsync(
                    id,
                    cancellationToken);

                VssConnection connection = GetConnection(service);
                _connections.Add(id, new AzureDevOpsConnectionContext(
                    service,
                    connection));
            }

            return _connections[id].Service;
        }

        private VssConnection GetConnection(
            AzureDevOpsConnectedService service)
        {
            var uri = new Uri($"https://dev.azure.com/{service.Account}");
            
            var credentials =
                new VssBasicCredential("", service.PersonalAccessToken);

            return new VssConnection(uri, credentials);
        }

        private async Task<AzureDevOpsConnectedService> GetConnectedServiceAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            IConnectedService? service = await _connectedServiceManager
                .GetAsync(id, cancellationToken);

            if ( service is AzureDevOpsConnectedService ado)
            {
                return ado;
            }

            throw new ApplicationException(
                $"No AzureDevOps ConnectedService " +
                 "found with name {name}");
        }
    }
}
