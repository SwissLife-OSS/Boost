using System;
using Boost.Infrastructure;
using Boost.Shell;
using Microsoft.AspNetCore.SignalR;

namespace Boost;

public class WebShellFactory : IWebShellFactory
{
    private readonly IHubContext<ConsoleHub> _hubContext;
    private readonly IDefaultShellService _defaultShellService;
    private readonly IToolManager _toolManager;
    private readonly IBoostApplicationContext _applicationContext;
    private readonly Action<ShellMessage> _messageHandler;

    public WebShellFactory(
        IHubContext<ConsoleHub> hubContext,
        IDefaultShellService defaultShellService,
        IToolManager toolManager,
        IBoostApplicationContext applicationContext)
    {
        _hubContext = hubContext;
        _defaultShellService = defaultShellService;
        _toolManager = toolManager;
        _applicationContext = applicationContext;
        _messageHandler = (msg) =>
        {
            _hubContext.Clients.All.SendAsync("consoleData", msg);
        };
    }

    public IWebShell CreateShell(string shell)
    {
        return new CliWrapWebShell(shell, _messageHandler, _toolManager, _applicationContext);
    }

    public IWebShell CreateShell()
    {
        return new CliWrapWebShell(
            _defaultShellService.GetDefault(),
            _messageHandler,
            _toolManager,
            _applicationContext);
    }
}

