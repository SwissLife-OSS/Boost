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
                    input.Scopes),
                cancellationToken);

            return new RequestTokenPayload(tokenResult);
        }

        public Task<Guid> SaveIdentityRequestAsync(
            [Service] IIdentityRequestStore requestStore,
            SaveIdentityRequestInput input,
            CancellationToken cancellationToken)
        {
            if (input.Type == IdentityRequestType.Token)
            {
                var data = new TokenRequestData(
                    input.Authority,
                    input.ClientId,
                    input.Secret,
                    input.GrantType!,
                    input.Scopes ?? new List<string>());

                var saveRequest = SaveIdentityRequest<TokenRequestData>.Create(data);

                saveRequest.Id = input.Id;
                saveRequest.Name = input.Name;
                saveRequest.Tags = input.Tags ?? Array.Empty<string>();
                saveRequest.Type = input.Type;
                saveRequest.Data = data;

                requestStore.Save(saveRequest);
            }

            return Task.FromResult(Guid.NewGuid());
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

    public record TokenRequestInput(
        string Authority,
        string ClientId,
        string Secret,
        string GrantType,
        IEnumerable<string> Scopes);

    public class StartAuthorizationRequestPayload
    {
        public StartAuthorizationRequestPayload(RunningWebServerInfo server)
        {
            Server = server;
        }

        public RunningWebServerInfo Server { get; }
    }

    public record SaveIdentityRequestInput(
        IdentityRequestType Type,
        string Name,
        string Authority,
        string ClientId)
    {
        public Guid? Id { get; init; }
        public string? Secret { get; init; }
        public string? GrantType { get; init; }
        public IEnumerable<string>? Scopes { get; init; }
        public IEnumerable<string>? Tags { get; init; }
        public bool? Pkce { get; init; }
        public int? Port { get; init; }
    }

    public class RequestTokenPayload
    {
        public RequestTokenPayload(RequestTokenResult token)
        {
            Result = token;
        }

        public RequestTokenResult Result { get; }
    }
}
