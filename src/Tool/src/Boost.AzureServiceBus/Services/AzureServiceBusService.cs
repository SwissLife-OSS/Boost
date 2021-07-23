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
    public class AzureServiceBusService : IAzureServiceBusService
    {
        private readonly ServiceBusAdministrationClient _administrationClient;

        public AzureServiceBusService(string connectionString)
        {
            _administrationClient = new ServiceBusAdministrationClient(connectionString);
        }

        public async Task<IReadOnlyList<QueueInfo>> GetQueuesAsync(
            CancellationToken cancellationToken)
        {
            var queueProperties = new List<QueueInfo>();

            Azure.AsyncPageable<QueueProperties> queuesPageable =
                _administrationClient.GetQueuesAsync(cancellationToken);

            IAsyncEnumerator<QueueProperties> enumerator = queuesPageable.GetAsyncEnumerator();

            try
            {
                while (await enumerator.MoveNextAsync())
                {
                    QueueProperties queue = enumerator.Current;
                    Azure.Response<QueueRuntimeProperties> runtimePropertiesResponse =
                        await _administrationClient.GetQueueRuntimePropertiesAsync(queue.Name, cancellationToken);

                    QueueRuntimeProperties runtimeProperties = runtimePropertiesResponse.Value;

                    queueProperties.Add(
                        new QueueInfo(
                            queue.Name,
                            queue.Status.ToString(),
                            runtimeProperties.ActiveMessageCount,
                            runtimeProperties.DeadLetterMessageCount));
                }
            }
            finally
            {
                await enumerator.DisposeAsync();
            }

            return queueProperties;
        }

        public async Task<IReadOnlyList<TopicInfo>> GetTopicsAsync(CancellationToken cancellationToken)
        {
            var topics = new List<TopicInfo>();

            Azure.AsyncPageable<TopicProperties> queuesPageable =
                _administrationClient.GetTopicsAsync(cancellationToken);

            IAsyncEnumerator<TopicProperties> enumerator = queuesPageable.GetAsyncEnumerator();

            try
            {
                while (await enumerator.MoveNextAsync())
                {
                    TopicProperties topic = enumerator.Current;
                    Azure.Response<TopicRuntimeProperties> runtimePropertiesResponse =
                        await _administrationClient.GetTopicRuntimePropertiesAsync(
                            topic.Name,
                            cancellationToken);

                    TopicRuntimeProperties runtimeProperties = runtimePropertiesResponse.Value;

                    topics.Add(new TopicInfo(topic.Name));
                }
            }
            finally
            {
                await enumerator.DisposeAsync();
            }

            return topics;
        }
    }
}
