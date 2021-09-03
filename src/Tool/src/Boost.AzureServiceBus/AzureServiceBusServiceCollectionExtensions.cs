using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boost.AzureServiceBus.GraphQL;
using Boost.AzureServiceBus.Services;
using Boost.AzureServiceBus.Settings;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.AzureServiceBus
{
    public static class AzureServiceBusServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureServiceBus(this IServiceCollection services)
        {
            services.AddSingleton<IAzureServiceBusService, AzureServiceBusService>();
            services.AddSingleton<IAzureServiceBusSettingsManager, AzureServiceBusSettingsManager>();

            return services;
        }

        public static IRequestExecutorBuilder AddAzureServiceBusTypes(
            this IRequestExecutorBuilder builder)
        {
            builder
                .AddType<AzureServiceBusQueries>()
                .AddType<AzureServiceBusMutations>();

            return builder;
        }
    }
}
