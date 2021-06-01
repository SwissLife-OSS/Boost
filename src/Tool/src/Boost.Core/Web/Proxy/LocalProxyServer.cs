using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Yarp.ReverseProxy.Abstractions;

namespace Boost.Web.Proxy
{
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
                    ProxyRoute[]? routes = new[]
                    {
                        new ProxyRoute()
                        {
                            RouteId = "route1",
                            ClusterId = "cluster1",
                            Match = new RouteMatch
                            {
                                Path = "{**catch-all}"
                            }
                        }
                    };

                    Cluster[] clusters = new[]
                    {
                        new Cluster()
                        {
                            Id = "cluster1",
                            Destinations = new Dictionary<string, Destination>(StringComparer.OrdinalIgnoreCase)
                            {
                                { "destination1", new Destination() { Address = options.DestinationAddress} }
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
}
