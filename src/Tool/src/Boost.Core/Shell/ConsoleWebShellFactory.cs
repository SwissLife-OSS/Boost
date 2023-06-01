using System;
using Boost.Infrastructure;
using Boost.Shell;
using McMaster.Extensions.CommandLineUtils;

namespace Boost
{
    public class ConsoleWebShellFactory : IWebShellFactory
    {
        private readonly IDefaultShellService _defaultShellService;
        private readonly IBoostApplicationContext _applicationContext;
        private readonly IToolManager _toolManager;
        private readonly Action<ShellMessage> _messageHandler;

        public ConsoleWebShellFactory(
            IDefaultShellService defaultShellService,
            IBoostApplicationContext applicationContext,
            IToolManager toolManager,
            IConsole console)
        {
            _defaultShellService = defaultShellService;
            _applicationContext = applicationContext;
            _toolManager = toolManager;

            _messageHandler = (msg) =>
            {
                if ( msg?.Message is { })
                {
                    console.WriteLine(msg.Message);
                }
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
}

