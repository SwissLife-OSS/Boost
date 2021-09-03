using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.AzureServiceBus.Settings
{
    public interface IAzureServiceBusSettingsManager
    {
        Task<AzureServiceBusConnection> GetByName(
            string connectionName,
            CancellationToken cancellationToken);

        Task SaveAsync(
            AzureServiceBusConnection serviceBusConnection,
            CancellationToken cancellationToken);

        Task<AzureServiceBusSettings> GetSettingsAsync(CancellationToken cancellationToken);
    }
}
