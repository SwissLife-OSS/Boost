using Boost.GraphQL;
using Boost.Snapshooter;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boost;

public static class SnapshooterServiceCollectionExtensions
{
    public static IServiceCollection AddSnapshooter(this IServiceCollection services)
    {
        services.AddSingleton<ISnapshooterService, SnapshooterService>();

        return services;
    }

    public static IRequestExecutorBuilder AddSnapshooterTypes(
        this IRequestExecutorBuilder builder)
    {
        builder
            .AddType<SnapshooterMutations>()
            .AddType<SnapshooterQueries>();

        return builder;
    }
}
