using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.Security
{
    public class AuthorizeRequestService : IAuthorizeRequestService
    {
        private readonly IAuthWebServer _authWebServer;

        public AuthorizeRequestService(
            IAuthWebServer authWebServer)
        {
            _authWebServer = authWebServer;
        }

        public async Task<RunningWebServerInfo> StartAuthorizeRequestAsync(
            AuthorizeRequestData request,
            CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();

            RunningWebServerInfo server = await _authWebServer.StartAsync(
                new StartWebServerOptions(
                Guid.NewGuid(), request.Port)
                {
                    Title = $"{request.Authority} ({request.ClienId})",
                    SetupAction = (services) =>
                    {
                        services.AddSingleton(request);
                    }
                }, cancellationToken);

            return server;
        }
    }
}
