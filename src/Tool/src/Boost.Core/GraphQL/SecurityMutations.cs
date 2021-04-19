using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Security;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.GraphQL
{
    [ExtendObjectType(Name = RootTypes.Mutation)]
    public class SecurityMutations
    {
        public async Task<StartAuthorizationRequestPayload> StartAuthorizationRequestAsync(
            [Service] IAuthorizeRequestService authService,
            AuthorizeRequestInput input,
            CancellationToken cancellationToken)
        {
            var request = new AuthorizeRequestData(
                input.Authority,
                input.ClientId,
                input.Secret,
                input.Scopes,
                input.Pkce)
            {
                Port = input.Port
            };

            RunningWebServerInfo session = await authService.StartAuthorizeRequestAsync(
                request,
                cancellationToken);

            return new StartAuthorizationRequestPayload(session);
        }

        public async Task<string> StartAuthServerAsync(
            [Service] IAuthWebServer authWebServer,
            StartAuthServerInput input,
            CancellationToken cancellationToken)
        {
            RunningWebServerInfo? server = await authWebServer.StartAsync(
                new StartWebServerOptions(Guid.NewGuid(), input.Port.GetValueOrDefault(3010)),
                cancellationToken);

            return server.Id.ToString();
        }

        public async Task<string> StopAuthServerAsync(
            [Service] IAuthWebServer authWebServer,
            Guid id,
            CancellationToken cancellationToken)
        {
            await authWebServer.StopAsync(id, cancellationToken);

            return id.ToString("N");
        }
    }

    public record AuthorizeRequestInput(
        string Authority,
        string ClientId,
        string Secret,
        IEnumerable<string> Scopes,
        bool Pkce)
    {
        public int Port { get; init; }
    }

    public class StartAuthorizationRequestPayload
    {
        public StartAuthorizationRequestPayload(RunningWebServerInfo server)
        {
            Server = server;
        }

        public RunningWebServerInfo Server { get; }
    }
}
