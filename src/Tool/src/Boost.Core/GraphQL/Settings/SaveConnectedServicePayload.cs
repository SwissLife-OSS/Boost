using Boost.Settings;

namespace Boost.Core.GraphQL;

public class SaveConnectedServicePayload
{
    public SaveConnectedServicePayload(ConnectedService connectedService)
    {
        ConnectedService = connectedService;
    }

    public ConnectedService ConnectedService { get; }
}
