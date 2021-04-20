using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.Security
{
    public interface IAuthWebServer
    {
        IEnumerable<RunningWebServerInfo> GetRunningServers();
        Task<RunningWebServerInfo> StartAsync(StartWebServerOptions options, CancellationToken cancellationToken);
        Task StopAsync(Guid id, CancellationToken cancellationToken);
    }

    public record StartWebServerOptions(Guid Id, int Port)
    {
        public string? Title { get; init; }

        public bool UseHttps { get; init; } = false;

        public Action<IServiceCollection>? SetupAction { get; init; }
    }

    public record RunningWebServerInfo(Guid Id, string Url)
    {
        public DateTime StartedAt { get; } = DateTime.UtcNow;

        public string? Title { get; init; }
    }
}
