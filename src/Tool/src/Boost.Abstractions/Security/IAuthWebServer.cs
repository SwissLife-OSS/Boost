using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Security;

public interface IAuthWebServer
{
    IEnumerable<RunningWebServerInfo> GetRunningServers();

    Task<RunningWebServerInfo> StartAsync(
        StartWebServerOptions options,
        CancellationToken cancellationToken);

    Task StopAsync(Guid id, CancellationToken cancellationToken);
}
