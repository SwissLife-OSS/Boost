using Boost.Certificates;
using Boost.Core.GraphQL;
using Boost.Core.Settings;
using Boost.Data;
using Boost.Git;
using Boost.GraphQL;
using Boost.Infrastructure;
using Boost.Navigation;
using Boost.Nuget;
using Boost.Pipelines;
using Boost.Security;
using Boost.Settings;
using Boost.Shell;
using Boost.Utils;
using Boost.Web.Proxy;
using Boost.Workspace;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boost;

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
        services.AddSingleton<IToolManager, ToolManager>();
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

        services.AddSingleton<ICertificateManager, CertificateManager>();
        services.AddSingleton<IIdentityRequestStore, LocalIdentityRequestStore>();
        services.AddSingleton<IIdentityService, IdentityService>();
        services.AddSingleton<IBoostDbContextFactory, BoostDbContextFactory>();
        services.AddSingleton<ISecurityUtils, SecurityUtils>();
        services.AddSingleton<IAuthTokenStore, UserDataAuthTokenStore>();
        services.AddSingleton<IAuthTokenStoreReader, UserDataAuthTokenStoreReader>();
        services.AddSingleton<IVersionChecker, VersionChecker>();
        services.AddSingleton<IAppNavigationService, AppNavigationService>();
        services.AddSingleton<ILocalProxyServer, LocalProxyServer>();

        services.AddUserDataProtection();

        return services;
    }

    public static IServiceCollection AddUserDataProtection(this IServiceCollection services)
    {
        services.AddSingleton<IDataProtector, CertificateDataProtector>();
        services.AddSingleton<IUserDataProtector, KeyRingUserDataProtector>();
        services.AddSingleton<ISymetricEncryption, SymetricEncryption>();

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
            .AddType<UserSettingsType>()
            .AddDataLoader<ConnectedServiceByIdDataLoader>();

        return builder;
    }
}
