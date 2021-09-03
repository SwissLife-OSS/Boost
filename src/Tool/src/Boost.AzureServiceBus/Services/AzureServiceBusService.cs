using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus.Administration;
using Boost.AzureServiceBus.Models;
using Boost.AzureServiceBus.Settings;

namespace Boost.AzureServiceBus.Services
{
    public class AzureServiceBusService : IAzureServiceBusService
    {
        private readonly IAzureServiceBusSettingsManager _serviceBusSettingsManager;

        public AzureServiceBusService(IAzureServiceBusSettingsManager serviceBusSettingsManager)
        {
            _serviceBusSettingsManager = serviceBusSettingsManager;
        }

        public async Task<IReadOnlyList<QueueInfo>> GetQueuesAsync(
            string connectionName,
            CancellationToken cancellationToken)
        {
            ServiceBusAdministrationClient administrationClient =
                await GetClientAsync(connectionName, cancellationToken);

            var queueProperties = new List<QueueInfo>();

            Azure.AsyncPageable<QueueProperties> queuesPageable =
                administrationClient.GetQueuesAsync(cancellationToken);

            IAsyncEnumerator<QueueProperties> enumerator = queuesPageable.GetAsyncEnumerator();

            try
            {
                while (await enumerator.MoveNextAsync())
                {
                    QueueProperties queue = enumerator.Current;
                    Azure.Response<QueueRuntimeProperties> runtimePropertiesResponse =
                        await administrationClient.GetQueueRuntimePropertiesAsync(
                            queue.Name,
                            cancellationToken);

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

        public async Task<IReadOnlyList<TopicInfo>> GetTopicsAsync(
            string connectionName,
            CancellationToken cancellationToken)
        {
            ServiceBusAdministrationClient administrationClient =
                await GetClientAsync(connectionName, cancellationToken);

            var topics = new List<TopicInfo>();

            Azure.AsyncPageable<TopicProperties> queuesPageable =
                administrationClient.GetTopicsAsync(cancellationToken);

            IAsyncEnumerator<TopicProperties> enumerator = queuesPageable.GetAsyncEnumerator();

            try
            {
                while (await enumerator.MoveNextAsync())
                {
                    TopicProperties topic = enumerator.Current;
                    Azure.Response<TopicRuntimeProperties> runtimePropertiesResponse =
                        await administrationClient.GetTopicRuntimePropertiesAsync(
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

        private async Task<ServiceBusAdministrationClient> GetClientAsync(
            string connectionName,
            CancellationToken cancellationToken)
        {
            AzureServiceBusConnection serviceBusConnection =
                await _serviceBusSettingsManager.GetByName(connectionName, cancellationToken);

            return new ServiceBusAdministrationClient(serviceBusConnection.ConnectionString);
        }
    }
}
