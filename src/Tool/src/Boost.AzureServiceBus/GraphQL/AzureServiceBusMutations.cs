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
        public async Task<SaveAzureServiceBusConnectionPayload> SaveAzureServiceBusConnectionAsync(
            AzureServiceBusConnection input,
            [Service] IAzureServiceBusSettingsManager settingsManager,
            CancellationToken cancellationToken)
        {
            //TODO: validate
            await settingsManager.SaveAsync(input, cancellationToken);
            return new SaveAzureServiceBusConnectionPayload(true);
        }
    }
}
