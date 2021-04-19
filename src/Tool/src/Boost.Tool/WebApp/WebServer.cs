using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Boost.AzureDevOps;
using Boost.Data;
using Boost.GitHub;
using Boost.GraphQL;
using Boost.Security;
using Boost.Tool;
using Boost.Tool.AuthApp;
using Boost.Web;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Boost
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

        public async Task StartAsync(int port)
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
                    services.AddControllersWithViews()
                        .AddRazorRuntimeCompilation();
                    services.AddSameSiteOptions();
                    services.AddHttpContextAccessor();
                    services.AddSignalR();
                    services.AddGraphQLServices(gql =>
                    {
                       gql.AddType<AzureDevOpsGitRemoteReference>();
                       gql.AddType<GitHubRemoteReference>();
                    });
                    services.AddBoost();
                    services.AddGitHub();
                    services.AddAzureDevOps();
                    services.AddSnapshooter();
                    services.AddSingleton<IWebShellFactory, WebShellFactory>();
                    services.AddHttpClient();

                    services.AddSingleton<IAuthWebServer, AuthWebServer>();

                    services.AddSingleton(
                        _commandContext.Services.GetRequiredService<IBoostDbContext>());
                })
                .Build();

            if (!Debugger.IsAttached)
            {
                Process browser = ProcessHelpers.OpenBrowser(url);
            }

            _console.WriteLine($"Boost server started on {url}");

            await _host.RunAsync();
        }

        public Task StopAsync()
        {
            return _host.StopAsync();
        }
    }
}
