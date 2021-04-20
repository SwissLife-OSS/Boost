using Boost.Security;

namespace Boost.GraphQL
{
    public class StartAuthorizationRequestPayload
    {
        public StartAuthorizationRequestPayload(RunningWebServerInfo server)
        {
            Server = server;
        }

        public RunningWebServerInfo Server { get; }
    }
}
