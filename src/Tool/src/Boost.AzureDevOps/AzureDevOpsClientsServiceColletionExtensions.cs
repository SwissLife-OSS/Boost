using Boost.Git;
using Boost.Pipelines;
using Boost.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.AzureDevOps
{
    public static class AzureDevOpsClientsServiceColletionExtensions
    {
        public static IServiceCollection AddAzureDevOps(
            this IServiceCollection services)
        {
            services.AddSingleton<AzureDevOpsClientFactory>();
            services.AddSingleton<AzureDevOpsClient>();
            services.AddSingleton<IAzureDevOpsGitClient, AzureDevOpsGitClient>();
            services.AddSingleton<IAzureDevOpsPipelinesClient, AzureDevOpsPipelinesClient>();
            services.AddSingleton<IGitRemoteClient, AzureDevOpsGitClient>();
            services.AddSingleton<IPipelinesClient, AzureDevOpsPipelinesClient>();

            services.AddSingleton<IConnectedServiceProvider, AzureDevOpsConnectedServiceProvider>();
            return services;
        }
    }
}
