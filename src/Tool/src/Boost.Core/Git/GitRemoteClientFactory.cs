using System;
using System.Collections.Generic;
using System.Linq;

namespace Boost.Git
{
    public class GitRemoteClientFactory : IGitRemoteClientFactory
    {
        private readonly IEnumerable<IGitRemoteClient> _remoteClients;

        public GitRemoteClientFactory(IEnumerable<IGitRemoteClient> remoteClients)
        {
            _remoteClients = remoteClients;
        }

        public IGitRemoteClient Create(string serviceType)
        {
            IGitRemoteClient? client = _remoteClients
                .SingleOrDefault(x => x.ConnectedServiceType == serviceType);

            if ( client is null)
            {
                throw new ApplicationException(
                    $"No client registred for service type: {serviceType}");
            }

            return client;
        }
    }
}
