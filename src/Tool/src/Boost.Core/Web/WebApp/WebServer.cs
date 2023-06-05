using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boost.AuthApp;
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

namespace Boost.WebApp;

public class BoostWebServer : IWebServer, IDisposable
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

    public string LogLevel { get; set; } = "warning";

    public async Task<string> StartAsync(int port)
    {
        LogConfiguration.CreateLogger(LogLevel);

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

                 services.AddSingleton(
                     _commandContext.Services!.GetRequiredService<AppSettings>());

             })
             .Build();

        await _host.StartAsync();
        _console.WriteLine($"Boost server started on {url}");

        return url;
    }

    public Task StopAsync()
    {
        return _host.StopAsync();
    }

    public void Dispose()
    {
        if (_host is { })
        {
            _host.Dispose();
        }
    }
}
