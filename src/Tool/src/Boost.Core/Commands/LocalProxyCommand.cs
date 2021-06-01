using System;
using System.Threading.Tasks;
using Boost.Infrastructure;
using Boost.Web;
using Boost.Web.Proxy;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.Commands
{
    [Command(
        Name = "lp",
        FullName = "Local proxy",
        Description = "Starts a local proxy server")]
    public class LocalProxyCommand : CommandBase
    {
        private readonly ILocalProxyServer _proxyServer;

        public LocalProxyCommand(ILocalProxyServer proxyServer)
        {
            _proxyServer = proxyServer;
        }

        [Option("--port <PORT>", Description = "Webserver port")]
        public int Port { get; set; } = 5001;

        [Argument(0, "DestinationAddress", ShowInHelpText = true)]
        public string DestinationAddress { get; set; } = default!;

        public async Task OnExecute(IConsole console)
        {
            console.WriteLine("Starting local proxy");
            var port = NetworkExtensions.GetAvailablePort(Port);

            if (port != Port)
            {
                console.WriteLine($"Port {Port} is allready in use.", ConsoleColor.Yellow);
                var useOther = Prompt.GetYesNo($"Start proxy on port: {port}", true);

                if (useOther)
                {
                    Port = port;
                }
                else
                {
                    return;
                }
            }
            var options = new LocalProxyOptions
            {
                Port = Port,
                DestinationAddress = DestinationAddress
            };

            string url = await _proxyServer.StartAsync(
                options,
                CommandAborded);

            ProcessHelpers.OpenBrowser(url);

            var stopMessage = "Press 'q' or 'esc' to stop";
            console.WriteLine(stopMessage);

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.KeyChar == 'q' || key.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else
                {
                    console.ClearLine();
                    console.WriteLine("Unknown command", ConsoleColor.Red);
                    Console.WriteLine(stopMessage);
                }
            }

            console.WriteLine("Stopping proxy....");
            await _proxyServer.StopAsync();

            _proxyServer.Dispose();
        }
    }
}
