using Boost.Core.GraphQL;
using Boost.Core.Settings;
using Boost.Data;
using Boost.Git;
using Boost.GraphQL;
using Boost.Infrastructure;
using Boost.Nuget;
using Boost.Pipelines;
using Boost.Security;
using Boost.Settings;
using Boost.Utils;
using Boost.Workspace;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boost
{
    public static class BoostServiceCollectionExtensions
    {
        public static IServiceCollection AddBoost(this IServiceCollection services)
        {
            services.AddSingleton<IDefaultShellService, DefaultShellService>();
            services.AddSingleton<IWorkspaceService, WorkspaceService>();
            services.AddSingleton<IEncodingService, EncodingService>();
            services.AddSingleton<ITokenAnalyzer, TokenAnalyzer>();
            services.AddSingleton<IBoostApplicationContext, BoostApplicationContext>();
            services.AddSingleton<ISettingsStore, SettingsStore>();
            services.AddSingleton<IUserSettingsManager, UserSettingsManager>();
            services.AddSingleton<IConnectedServiceManager, ConnectedServiceManager>();
            services.AddSingleton<IGitRemoteSearchService, GitRemoteSearchService>();
            services.AddSingleton<IGitRemoteClientFactory, GitRemoteClientFactory>();
            services.AddSingleton<IGitRemoteService, GitRemoteService>();
            services.AddSingleton<ILocalRepositoryIndexer, LocalRepositoryIndexer>();
            services.AddSingleton<IGitLocalRepositoryService, GitLocalRepositoryService>();
            services.AddSingleton<IPipelinesService, PipelinesService>();
            services.AddSingleton<IFileContentTypeHandler, DefaultContentTypeHandler>();
            services.AddSingleton<IFileContentTypeHandler, ImageContentTypeHandler>();
            services.AddSingleton<IFileContentTypeHandler, PdfContentTypeHandler>();
            services.AddSingleton<IFileContentTypeHandler, DllContentTypeHandler>();
            services.AddSingleton<IAuthorizeRequestService, AuthorizeRequestService>();
            services.AddNuget();

            services.AddHttpClient("IDENTITY");
            services.AddSingleton<IUserDataProtector, NoOpDataProtector>();
            services.AddSingleton<IIdentityRequestStore, LocalIdentityRequestStore>();
            services.AddSingleton<IIdentityService, IdentityService>();
            services.AddSingleton<IBoostDbContextFactory, BoostDbContextFactory>();
            services.AddSingleton<ISecurityUtils, SecurityUtils>();
            services.AddSingleton<IAuthTokenStore, UserDataAuthTokenStore>();
            services.AddSingleton<IAuthTokenStoreReader, UserDataAuthTokenStoreReader>();

            return services;
        }

        public static IServiceCollection AddNuget(this IServiceCollection services)
        {
            services.AddSingleton<INugetService, NugetService>();

            return services;
        }

        public static IRequestExecutorBuilder AddBoostTypes(
            this IRequestExecutorBuilder builder)
        {
            builder
                .AddType<BoostQueries>()
                .AddType<SettingsQueries>()
                .AddType<PipelinesQueries>()
                .AddType<WorkspaceQueries>()
                .AddType<WorkspaceMutations>()
                .AddType<ShellMutations>()
                .AddType<UtilsQueries>()
                .AddType<GitQueries>()
                .AddType<SecurityQueries>()
                .AddType<SecurityMutations>()
                .AddType<GitMutations>()
                .AddType<SettingsMutations>()
                .AddType<NugetQueries>()
                .AddType<GitRemoteRepositoryType>()
                .AddType<LocalGitRepositoryType>()
                .AddType<PipelineType>()

                .AddDataLoader<ConnectedServiceByIdDataLoader>();

            return builder;
        }
    }
}
