using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Security;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.GraphQL
{
    [ExtendObjectType(RootTypes.Mutation)]
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
                input.UsePkce)
            {
                Port = input.Port,
                SaveTokens = input.SaveTokens,
                RequestId = input.RequestId
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

        public async Task<RequestTokenPayload> RequestTokenAsync(
            [Service] IIdentityService identityService,
            TokenRequestInput input,
            CancellationToken cancellationToken)
        {
            RequestTokenResult tokenResult = await identityService.RequestTokenAsync(
                new TokenRequestData(
                    input.Authority,
                    input.ClientId,
                    input.Secret,
                    input.GrantType,
                    input.Scopes,
                    input.Parameters),
                cancellationToken);

            return new RequestTokenPayload(tokenResult);
        }

        public async Task<SaveIdentityRequestPayload> SaveIdentityRequestAsync(
            [Service] IIdentityRequestStore requestStore,
            SaveIdentityRequestInput input,
            CancellationToken cancellationToken)
        {
            IdentityRequestItem item = await requestStore
                .SaveAsync(input, cancellationToken);

            return new SaveIdentityRequestPayload(item);
        }

        public async Task<Guid> DeleteIdentityRequestAsync(
            [Service] IIdentityRequestStore requestStore,
            Guid id,
            CancellationToken cancellationToken)
        {
            await requestStore.DeleteAsync(id, cancellationToken);

            return id;
        }
    }
}
