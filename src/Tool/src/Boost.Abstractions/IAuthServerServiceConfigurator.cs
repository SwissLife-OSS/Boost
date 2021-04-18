using Microsoft.Extensions.DependencyInjection;

namespace Boost
{
    public interface IAuthServerServiceConfigurator
    {
        IServiceCollection Configure(IServiceCollection services);
    }
}
