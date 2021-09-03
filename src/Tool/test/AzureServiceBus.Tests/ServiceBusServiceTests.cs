using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus.Administration;
using Boost;
using Boost.AzureServiceBus.Models;
using Boost.AzureServiceBus.Services;
using Snapshooter.Xunit;
using Xunit;

namespace AzureServiceBus.Tests
{
    public class ServiceBusServiceTests
    {
        [Fact]
        public async Task GetQueuesAsync_CorrectConnectionString_RetrievesAllQueues()
        {
            // Arrange
            //var sut = new AzureServiceBusService();

            //// Act
            //IReadOnlyList<QueueInfo>? items = await sut.GetQueuesAsync(default);

            //foreach (QueueInfo item in items)
            //{

            //}

            // Assert
            //queues.MatchSnapshot();
        }
    }
}
