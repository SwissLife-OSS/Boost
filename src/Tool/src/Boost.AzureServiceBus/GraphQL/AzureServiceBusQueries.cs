using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Boost.AzureServiceBus.Models;
using Boost.AzureServiceBus.Services;
using Boost.GraphQL;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.AzureServiceBus.GraphQL
{
    [ExtendObjectType(Name = RootTypes.Query)]
    public class AzureServiceBusQueries
    {
        public async Task<IEnumerable<QueueInfo>> GetAzureServiceBusQueuesAsync(
            [Service] IAzureServiceBusService azureServiceBusService,
            CancellationToken cancellationToken)
        {
            return await azureServiceBusService.GetQueuesAsync(cancellationToken);
        }
    }
}
