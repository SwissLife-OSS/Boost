using System;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Web.Proxy
{
    public interface ILocalProxyServer : IDisposable
    {
        Task<string> StartAsync(LocalProxyOptions options, CancellationToken cancellationToken);
        Task StopAsync();
    }
}
