using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.GitHub;

public interface IGitHubAuthServer
{
    Task StartAsync(int port, Guid id, Action<IServiceCollection> configure);
}
