using Boost.AzureDevOps;
using Boost.GitHub;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.Tool
{
    public static class ToolServiceCollectionExtensions
    {
        public static IServiceCollection AddToolServices(this IServiceCollection services)
        {
            services.AddBoost();
            services.AddSnapshooter();
            services.AddAzureDevOps();
            services.AddGitHub();
            services.AddSingleton<IAuthServerServiceConfigurator, AuthServerServiceConfigurator>();

            return services;
        }
    }

    public class AuthServerServiceConfigurator : IAuthServerServiceConfigurator
    {
        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddBoost();

            return services;
        }
    }
}
