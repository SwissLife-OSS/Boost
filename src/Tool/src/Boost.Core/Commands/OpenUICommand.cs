using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Boost.Infrastructure;
using Boost.Web;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.Commands;

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

    [Option("--path <PATH>", Description = "Url path")]
    public string? Path { get; set; }

    [Option("--log <level>", Description = "Log level: debug|info|warning|error")]
    public string LogLevel { get; set; } = "warning";

    public async Task OnExecute(IConsole console)
    {
        console.WriteLine("Starting Boost UI...");
        var port = NetworkExtensions.GetAvailablePort(Port);

        if (port != Port)
        {
            console.WriteLine($"Port {Port} is allready in use.", ConsoleColor.Yellow);
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
        _webServer.LogLevel = LogLevel;

        var url = await _webServer.StartAsync(Port);

        if (!Debugger.IsAttached)
        {
            if (Path is { })
            {
                url = url + $"/{Path}";
            }

            ProcessHelpers.OpenBrowser(url);
        }
        var stopMessage = "Press 'q' or 'esc' to stop";
        Console.WriteLine(stopMessage);

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

        console.WriteLine("Stopping boost....");
        await _webServer.StopAsync();

        _webServer.Dispose();
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
