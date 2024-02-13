using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace Boost.Web.Proxy;

public static class InMemoryConfigProviderExtensions
{
    public static IReverseProxyBuilder LoadFromMemory(
        this IReverseProxyBuilder builder,
        IReadOnlyList<RouteConfig> routes,
        IReadOnlyList<ClusterConfig> clusters)
    {
        builder.Services.AddSingleton<IProxyConfigProvider>(
            new InMemoryConfigProvider(routes, clusters));

        return builder;
    }
}
public class InMemoryConfigProvider : IProxyConfigProvider
{
    private volatile InMemoryConfig _config;

    public InMemoryConfigProvider(
        IReadOnlyList<RouteConfig> routes,
        IReadOnlyList<ClusterConfig> clusters)
    {
        _config = new InMemoryConfig(routes, clusters);
    }

    public IProxyConfig GetConfig() => _config;

    public void Update(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
    {
        InMemoryConfig oldConfig = _config;
        _config = new InMemoryConfig(routes, clusters);
        oldConfig.SignalChange();
    }

    private class InMemoryConfig : IProxyConfig
    {
        private readonly CancellationTokenSource _cts = new();

        public InMemoryConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
        {
            Routes = routes;
            Clusters = clusters;
            ChangeToken = new CancellationChangeToken(_cts.Token);
        }

        public IReadOnlyList<RouteConfig> Routes { get; }

        public IReadOnlyList<ClusterConfig> Clusters { get; }

        public IChangeToken ChangeToken { get; }

        internal void SignalChange()
        {
            _cts.Cancel();
        }
    }
}
