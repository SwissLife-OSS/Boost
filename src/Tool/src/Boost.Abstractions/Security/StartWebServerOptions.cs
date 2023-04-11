using System;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.Security
{
    public record StartWebServerOptions(Guid Id, int? Port)
    {
        public string? Title { get; init; }

        public bool UseHttps { get; init; } = false;

        public Action<IServiceCollection>? SetupAction { get; init; }
    }
}
