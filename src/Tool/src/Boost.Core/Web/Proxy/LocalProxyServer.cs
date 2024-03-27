using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Yarp.ReverseProxy;
using Yarp.ReverseProxy.Configuration;

namespace Boost.Web.Proxy;

public class LocalProxyServer : ILocalProxyServer, IDisposable
{
    private IHost _host = default!;

    public async Task<string> StartAsync(
        LocalProxyOptions options,
        CancellationToken cancellationToken)
    {
        var url = $"https://localhost:{options.Port}";

        _host = Host.CreateDefaultBuilder()
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls(url);
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureServices((ctx, services) =>
            {
                RouteConfig[] routes =
                [
                    new RouteConfig()
                    {
                        RouteId = "route1",
                        ClusterId = "cluster1",
                        Match = new RouteMatch
                        {
                            Path = "{**catch-all}"
                        }
                    }
                ];

                ClusterConfig[] clusters = new[]
                {
                    new ClusterConfig()
                    {
                        ClusterId = "cluster1",
                        Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
                        {
                            { "destination1", new DestinationConfig() { Address =options.DestinationAddress } },
                        }
                    }
                };

                services.AddReverseProxy()
                    .LoadFromMemory(routes, clusters);
            })

            .Build();

        await _host.StartAsync(cancellationToken);

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
