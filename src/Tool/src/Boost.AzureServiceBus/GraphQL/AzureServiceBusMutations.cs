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
    [ExtendObjectType(Name = RootTypes.Mutation)]
    public class AzureServiceBusMutations
    {
        public async Task<AzureServiceBusPayload> SaveAzureServiceBusConnectionAsync(
            AzureServiceBusConnection input,
            [Service] IAzureServiceBusSettingsManager settingsManager,
            CancellationToken cancellationToken)
        {
            //TODO: validate
            await settingsManager.SaveConnectionAsync(input, cancellationToken);
            return new AzureServiceBusPayload(true);
        }

        public async Task<AzureServiceBusPayload> DeleteAzureServiceBusConnectionAsync(
            string connectionName,
            [Service] IAzureServiceBusSettingsManager settingsManager,
            CancellationToken cancellationToken)
        {
            await settingsManager.DeleteConnectionByNameAsync(connectionName, cancellationToken);
            return new AzureServiceBusPayload(true);
        }
    }
}
