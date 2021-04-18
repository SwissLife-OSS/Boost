using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.Settings;
using Octokit;

namespace Boost.GitHub
{
    public class GitHubClientFactory
    {
        private readonly Dictionary<Guid, GitHubConnectionContext> _connections
            = new Dictionary<Guid, GitHubConnectionContext>();
        private readonly IConnectedServiceManager _connectedServiceManager;



        public GitHubClientFactory(IConnectedServiceManager connectedServiceManager)
        {
            _connectedServiceManager = connectedServiceManager;
        }
        internal Dictionary<Guid, GitHubConnectionContext> Connections => _connections;

        public async Task<GitHubConnectedService> ConnectAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            if (!_connections.ContainsKey(id))
            {
                GitHubConnectedService service = await GetConnectedServiceAsync(
                    id,
                    cancellationToken);

                var client = new GitHubClient(new ProductHeaderValue("boost"));
                client.Credentials = new Credentials(service.AccessToken);

                _connections.Add(id, new GitHubConnectionContext(service, client));
            }

            return _connections[id].Service;
        }

        public GitHubClient CreateClient(Guid id)
        {
            return _connections[id].Client;
        }

        private async Task<GitHubConnectedService> GetConnectedServiceAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            IConnectedService? service = await _connectedServiceManager
                .GetAsync(id, cancellationToken);

            if (service is GitHubConnectedService gh)
            {
                return gh;
            }

            throw new ApplicationException(
                $"No GitHub ConnectedService " +
                 "found with name {name}");
        }
    }
}
