using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Boost.AuthApp;
using Boost.Core.GraphQL;
using Boost.Data;
using Boost.Infrastructure;
using Boost.Security;
using Boost.Web;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Boost.WebApp
{
    public class BoostWebServer : IWebServer
    {
        private readonly IConsole _console;
        private readonly BoostCommandContext _commandContext;
        private IHost _host = default!;

        public BoostWebServer(
            IConsole console,
            BoostCommandContext commandContext)
        {
            _console = console;
            _commandContext = commandContext;
        }

        public async Task StartAsync(int port, string? path = null)
        {
            LogConfiguration.CreateLogger();

            var url = $"http://localhost:{port}";

            _host = Host.CreateDefaultBuilder()
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(url);
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string>()
                    {
                        ["Boost:Port"] = port.ToString(),
                        ["Boost:WebServerUrl"] = url
                    });
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddControllers()
                        .PartManager.ApplicationParts.Add(new AssemblyPart(_commandContext.ToolAssembly));

                    services.AddSameSiteOptions();
                    services.AddHttpContextAccessor();
                    services.AddSignalR();
                    services.AddBoost();
                    services.AddSingleton<IBoostCommandContext>(_commandContext);

                    services.AddSingleton<IWebShellFactory, WebShellFactory>();
                    services.AddHttpClient();
                    services.AddSingleton<IAuthWebServer, AuthWebServer>();
                    _commandContext.ConfigureWeb?.Invoke(services);

                    services.AddSingleton(
                        _commandContext.Services!.GetRequiredService<IBoostDbContextFactory>());

                    services.AddSingleton(
                        _commandContext.Services!.GetRequiredService<IUserDataProtector>());

                })
                .Build();

            if (!Debugger.IsAttached)
            {
                if (path is { })
                {
                    url = url + $"/{path}";
                }

                Process browser = ProcessHelpers.OpenBrowser(url);
            }

            _console.WriteLine($"Boost server started on {url}");
            _console.WriteLine("Press CTRL + C to stop...");

            await _host.RunAsync();
        }

        public Task StopAsync()
        {
            return _host.StopAsync();
        }
    }
}
