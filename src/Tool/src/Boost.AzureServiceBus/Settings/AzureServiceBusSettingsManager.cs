using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.Infrastructure;

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

        public async Task<AzureServiceBusConnection> GetByName(
            string connectionName,
            CancellationToken cancellationToken)
        {
            AzureServiceBusSettings settings = await GetSettingsAsync(cancellationToken);

            return settings.Connections
                .First(p => p.Name.ToLowerInvariant().Equals(connectionName.ToLowerInvariant()));
        }

        public async Task SaveAsync(
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

            if(settings == null)
            {
                settings = new AzureServiceBusSettings();
            }

            return settings;
        }
    }
}
