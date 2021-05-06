using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.Infrastructure
{
    public class BoostCommandContext : IBoostCommandContext
    {
        public BoostCommandContext(Assembly toolAssembly)
        {
            ToolAssembly = toolAssembly;
        }

        public BoostCommandContext(
            IServiceProvider services,
            Action<IServiceCollection> configureWeb,
            Assembly toolAssembly)
        {
            Services = services;
            ConfigureWeb = configureWeb;
            ToolAssembly = toolAssembly;
        }

        public IServiceProvider? Services { get; }
        public Action<IServiceCollection>? ConfigureWeb { get; }
        public Assembly ToolAssembly { get; }
    }

    public class AppSettings
    {
        public string PackageId { get; set; } = "Boost.Tool";
    }
}
