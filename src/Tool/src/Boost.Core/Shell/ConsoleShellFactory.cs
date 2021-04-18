using System;
using Boost.Infrastructure;

namespace Boost.Shell
{
    public class ConsoleShellFactory : IWebShellFactory
    {
        private IDefaultShellService _defaultShellService;
        private readonly IBoostApplicationContext _boostApplicationContext;
        private Action<ShellMessage> _messageHandler;

        public ConsoleShellFactory(
            IDefaultShellService defaultShellService,
            IBoostApplicationContext boostApplicationContext)
        {
            _defaultShellService = defaultShellService;
            _boostApplicationContext = boostApplicationContext;
            _messageHandler = (msg) =>
            {
                if (msg.Type == "info" || msg.Type == "error")
                {
                    Console.WriteLine(msg.Message);
                }
            };
        }

        public IWebShell CreateShell(string shell)
        {
            throw new NotImplementedException();
        }

        public IWebShell CreateShell()
        {
            return new WebShell(
                _defaultShellService.GetDefault(),
                _messageHandler,
                _boostApplicationContext);
        }
    }
}
