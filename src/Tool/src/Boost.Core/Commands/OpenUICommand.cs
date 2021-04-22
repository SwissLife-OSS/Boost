using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Boost.Infrastructure;
using Boost.Web;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.Commands
{
    [Command(
        Name = "ui",
        FullName = "Open UI",
        Description = "Opens the boost UI")]
    public class OpenUICommand : CommandBase
    {
        private readonly IWebServer _webServer;

        public OpenUICommand(IWebServer webServer)
        {
            _webServer = webServer;
        }

        [Option("--port <PORT>", Description = "Webserver port")]
        public int Port { get; set; } = 3003;

        public async Task OnExecute(IConsole console)
        {
            console.WriteLine("Starting Boost UI...");

            var port = NetworkExtensions.GetAvailablePort(Port);

            if (port != Port)
            {
                console.WriteLine($"Port {Port} is allready in use.", ConsoleColor.Red);
                var useOther = Prompt.GetYesNo($"Start UI on port: {port}", true);

                if (useOther)
                {
                    Port = port;
                }
                else
                {
                    return;
                }
            }

            console.WriteLine("Press CTRL + C to stop...");
            await _webServer.StartAsync(Port);
        }
    }

    [Command(
        Name = "login",
        FullName = "Login",
        Description = "Login to Boost tools"), HelpOption]
    public class LoginCommand
    {
        public void OnExecute(IConsole console)
        {
            console.WriteLine("Starting Boost UI...");
        }
    }
}
