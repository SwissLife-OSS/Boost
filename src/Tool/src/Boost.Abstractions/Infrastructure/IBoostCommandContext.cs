using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.Infrastructure
{
    public interface IBoostCommandContext
    {
        Action<IServiceCollection>? ConfigureWeb { get; }
        IServiceProvider? Services { get; }
        Assembly ToolAssembly { get; }
    }
}