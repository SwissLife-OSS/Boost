using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boost.AzureServiceBus.GraphQL;
using Boost.AzureServiceBus.Services;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.AzureServiceBus
{
    public static class AzureServiceBusServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureServiceBus(this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("SERVICEBUS_CONNECTIONSTRING");

            services.AddSingleton<IAzureServiceBusService>(new AzureServiceBusService(connectionString!));

            return services;
        }

        public static IRequestExecutorBuilder AddAzureServiceBusTypes(
            this IRequestExecutorBuilder builder)
        {
            builder
                .AddType<AzureServiceBusQueries>();

            return builder;
        }
    }
}
