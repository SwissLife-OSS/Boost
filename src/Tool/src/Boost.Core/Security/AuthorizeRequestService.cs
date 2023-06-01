using System;
using System.Threading;
using System.Threading.Tasks;
using Boost.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.Security;

public class AuthorizeRequestService : IAuthorizeRequestService
{
    private readonly IAuthWebServer _authWebServer;
    private readonly IBoostCommandContext _commandContext;

    public AuthorizeRequestService(
        IAuthWebServer authWebServer,
        IBoostCommandContext commandContext)
    {
        _authWebServer = authWebServer;
        _commandContext = commandContext;
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
                Title = $"{request.Authority} ({request.ClientId})",
                SetupAction = (services) =>
                {
                    services.AddSingleton(request);
                    services.AddSingleton<IBoostCommandContext>(_commandContext);
                }
            }, cancellationToken);

        return server;
    }
}
