using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.AzureServiceBus.Models;

namespace Boost.AzureServiceBus.Services
{
    public interface IAzureServiceBusService
    {
        Task<IReadOnlyList<QueueInfo>> GetQueuesAsync(
            string connectionName,
            CancellationToken cancellationToken);
        Task<IReadOnlyList<TopicInfo>> GetTopicsAsync(
            string connectionName,
            CancellationToken cancellationToken);
    }
}
