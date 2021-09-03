using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Boost.AzureServiceBus.Models;
using Boost.AzureServiceBus.Services;
using Boost.AzureServiceBus.Settings;
using Boost.GraphQL;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.AzureServiceBus.GraphQL
{
    [ExtendObjectType(Name = RootTypes.Query)]
    public class AzureServiceBusQueries
    {
        public async Task<IEnumerable<QueueInfo>> GetAzureServiceBusQueuesAsync(
            string connectionName,
            [Service] IAzureServiceBusService azureServiceBusService,
            CancellationToken cancellationToken)
        {
            return await azureServiceBusService.GetQueuesAsync(connectionName, cancellationToken);
        }

        public async Task<IEnumerable<TopicInfo>> GetAzureServiceBusTopicsAsync(
            string connectionName,
            [Service] IAzureServiceBusService azureServiceBusService,
            CancellationToken cancellationToken)
        {
            return await azureServiceBusService.GetTopicsAsync(connectionName, cancellationToken);
        }

        public async Task<AzureServiceBusSettings> GetAzureServiceBusSettings(
            [Service] IAzureServiceBusSettingsManager settingsManager,
            CancellationToken cancellationToken)
        {
            return await settingsManager.GetSettingsAsync(cancellationToken);
        }
    }
}
