using System;
using Boost.GraphQL;
using Boost.Infrastructure;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.Core.GraphQL
{
    [ExtendObjectType(RootTypes.Query)]
    public class BoostQueries
    {
        private readonly IBoostApplicationContext _boostApplicationContext;

        public BoostQueries(IBoostApplicationContext boostApplicationContext)
        {
            _boostApplicationContext = boostApplicationContext;
        }

        public BoostApplication GetAppliation()
        {
            var app = new BoostApplication
            {
                WorkingDirectory = _boostApplicationContext.WorkingDirectory.FullName,
                Version = _boostApplicationContext.Version,
                ConfigurationRequired = true,
            };

            return app;
        }
    }

    public static class GraphQLServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQLServices(
            this IServiceCollection services,
            Action<IRequestExecutorBuilder>? configure = null)
        {
            IRequestExecutorBuilder builder = services.AddGraphQLServer()
                .AddQueryType(d => d.Name(RootTypes.Query))
                .AddMutationType(d => d.Name(RootTypes.Mutation))
                .AddBoostTypes();

            configure?.Invoke(builder);

            return services;
        }
    }
}
