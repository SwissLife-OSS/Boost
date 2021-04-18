using System;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.GraphQL
{
    public static class GraphQLServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQLServices(
            this IServiceCollection services,
            Action<IRequestExecutorBuilder>? configure=null)
        {
            IRequestExecutorBuilder builder = services.AddGraphQLServer()
                .AddQueryType(d => d.Name(RootTypes.Query))
                .AddMutationType(d => d.Name(RootTypes.Mutation))
                .AddBoostTypes()
                .AddSnapshooterTypes();

            configure?.Invoke(builder);

            return services;
        }
    }
}
