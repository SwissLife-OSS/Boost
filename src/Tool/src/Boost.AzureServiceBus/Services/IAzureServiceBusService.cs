using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus.Administration;
using Boost.AzureServiceBus.Models;

namespace Boost.AzureServiceBus.Services
{
    public interface IAzureServiceBusService
    {
        Task<IReadOnlyList<QueueInfo>> GetQueuesAsync(CancellationToken cancellationToken);
    }
}
