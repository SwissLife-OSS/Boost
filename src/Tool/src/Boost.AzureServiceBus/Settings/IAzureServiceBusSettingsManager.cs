using System.Threading;
using System.Threading.Tasks;

namespace Boost.AzureServiceBus.Settings
{
    public interface IAzureServiceBusSettingsManager
    {
        Task<AzureServiceBusConnection> GetConnectionByName(
            string connectionName,
            CancellationToken cancellationToken);

        Task SaveConnectionAsync(
            AzureServiceBusConnection serviceBusConnection,
            CancellationToken cancellationToken);

        Task DeleteConnectionByNameAsync(
            string connectionName,
            CancellationToken cancellationToken);

        Task<AzureServiceBusSettings> GetSettingsAsync(CancellationToken cancellationToken);
    }
}
