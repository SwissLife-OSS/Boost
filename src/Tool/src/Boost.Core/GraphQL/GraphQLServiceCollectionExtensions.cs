using System;
using Boost.GraphQL;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.Core.GraphQL;

public static class GraphQLServiceCollectionExtensions
{
    public static IServiceCollection AddGraphQLServices(
        this IServiceCollection services,
        Action<IRequestExecutorBuilder>? configure = null)
    {
        IRequestExecutorBuilder builder = services.AddGraphQLServer()
            .ModifyRequestOptions(m => m.IncludeExceptionDetails = true)
            .AddQueryType(d => d.Name(RootTypes.Query))
            .AddMutationType(d => d.Name(RootTypes.Mutation))
            .ConfigureSchema(x => { })
            .AddBoostTypes();

        configure?.Invoke(builder);

        return services;
    }
}
