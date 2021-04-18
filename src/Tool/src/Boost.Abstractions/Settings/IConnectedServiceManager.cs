using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Git;
using Boost.Settings;

namespace Boost.Core.Settings
{
    public interface IConnectedServiceManager
    {
        Task<IConnectedService?> GetAsync(Guid id, CancellationToken cancellationToken);
        IGitRemoteReference? GetGitRemoteReference(IEnumerable<string> urls);
        Task<ConnectedService?> GetServiceAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<IConnectedService>> GetServicesAsync(CancellationToken cancellationToken);
        Task<IEnumerable<IConnectedService>> GetServicesByFeatureAsync(ConnectedServiceFeature feature, CancellationToken cancellationToken);
        IEnumerable<ConnectedServiceType> GetServiceTypes();
        IConnectedService? MatchServiceFromGitRemote(IGitRemoteReference? gitRemoteReference, IEnumerable<IConnectedService> connectedServices);
        Task<ConnectedService> SaveAsync(ConnectedService connectedService, CancellationToken cancellationToken);
    }
}
