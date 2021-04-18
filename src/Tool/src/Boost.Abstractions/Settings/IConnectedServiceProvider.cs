using System.Collections.Generic;
using Boost.Git;

namespace Boost.Settings
{
    public interface IConnectedServiceProvider
    {
        ConnectedServiceType Type { get; }

        IConnectedService MapService(ConnectedService service);
        IConnectedService? MatchServiceFromGitRemoteReference(IGitRemoteReference remoteReference, IEnumerable<IConnectedService> connectedServices);
        IGitRemoteReference? ParseRemoteUrl(IEnumerable<string> urls);
    }
}
