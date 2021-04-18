using System;
using Boost.Infrastructure;
using Boost.Shell;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol;

namespace Boost
{
    public class WebShellFactory : IWebShellFactory
    {
        private readonly IHubContext<ConsoleHub> _hubContext;
        private readonly IDefaultShellService _defaultShellService;
        private readonly IBoostApplicationContext _applicationContext;
        private Action<ShellMessage> _messageHandler;

        public WebShellFactory(
            IHubContext<ConsoleHub> hubContext,
            IDefaultShellService defaultShellService,
            IBoostApplicationContext applicationContext)
        {
            _hubContext = hubContext;
            _defaultShellService = defaultShellService;
            _applicationContext = applicationContext;
            _messageHandler = (msg) =>
            {
                _hubContext.Clients.All.SendAsync("consoleData", msg);
            };
        }

        public IWebShell CreateShell(string shell)
        {
            return new CliWrapWebShell(shell, _messageHandler, _applicationContext);
        }

        public IWebShell CreateShell()
        {
            return new CliWrapWebShell(
                _defaultShellService.GetDefault(),
                _messageHandler,
                _applicationContext);
        }
    }
}

