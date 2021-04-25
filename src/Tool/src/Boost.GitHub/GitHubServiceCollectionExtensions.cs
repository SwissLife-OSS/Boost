using Boost.Git;
using Boost.Settings;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.GitHub
{
    public static class GitHubServiceCollectionExtensions
    {
        public static IServiceCollection AddGitHub(this IServiceCollection services)
        {
            services.AddSingleton<IGitHubAuthServer, GitHubAuthServer>();
            services.AddSingleton<IConnectedServiceProvider, GitHubConnectedServiceProvider>();
            services.AddSingleton<GitHubClientFactory>();
            services.AddSingleton<IGitRemoteClient, GitHubRepoClient>();

            return services;
        }

        public static IRequestExecutorBuilder AddGitHubTypes(
            this IRequestExecutorBuilder builder)
        {
            builder
                .AddType<GitHubRemoteReference>();

            return builder;
        }
    }
}
