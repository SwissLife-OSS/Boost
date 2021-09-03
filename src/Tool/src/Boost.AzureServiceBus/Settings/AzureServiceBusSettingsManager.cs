using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;

namespace Boost.AzureServiceBus.Settings
{
    public class AzureServiceBusSettingsManager : IAzureServiceBusSettingsManager
    {
        private readonly ISettingsStore _settingsStore;
        internal static readonly string AzureServiceBusSettingsFileName =
            "AzureServiceBusSettings";

        public AzureServiceBusSettingsManager(
            ISettingsStore settingsStore)
        {
            _settingsStore = settingsStore;
        }

        public async Task<AzureServiceBusConnection> GetConnectionByName(
            string connectionName,
            CancellationToken cancellationToken)
        {
            AzureServiceBusSettings settings = await GetSettingsAsync(cancellationToken);

            return settings.Connections
                .First(p => p.Name.ToLowerInvariant().Equals(connectionName.ToLowerInvariant()));
        }

        public async Task DeleteConnectionByNameAsync(
            string connectionName,
            CancellationToken cancellationToken)
        {
            AzureServiceBusSettings settings = await GetSettingsAsync(cancellationToken);
            settings.RemoveConnection(connectionName);

            await _settingsStore.SaveProtectedAsync(
                settings,
                AzureServiceBusSettingsFileName,
                cancellationToken: cancellationToken);
        }

        public async Task SaveConnectionAsync(
            AzureServiceBusConnection serviceBusConnection,
            CancellationToken cancellationToken)
        {
            AzureServiceBusSettings settings = await GetSettingsAsync(cancellationToken);

            settings.AddOrReplaceConnection(serviceBusConnection);

            await _settingsStore.SaveProtectedAsync(
                settings,
                AzureServiceBusSettingsFileName,
                cancellationToken: cancellationToken);
        }

        public async Task<AzureServiceBusSettings> GetSettingsAsync(CancellationToken cancellationToken)
        {
            AzureServiceBusSettings? settings =
                await _settingsStore.GetProtectedAsync<AzureServiceBusSettings>(
                    AzureServiceBusSettingsFileName,
                    cancellationToken: cancellationToken);

            if (settings == null)
            {
                settings = new AzureServiceBusSettings();
            }

            return settings;
        }
    }
}
