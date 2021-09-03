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
    public class ServiceBusSettingsManager : IServiceBusSettingsManager
    {
        private readonly ISettingsStore _settingsStore;
        private readonly IUserDataProtector _userDataProtector;
        internal static readonly string ServiceBusSettingsFileName = "ServiceBusSettings";

        public ServiceBusSettingsManager(
            ISettingsStore settingsStore,
            IUserDataProtector userDataProtector)
        {
            _settingsStore = settingsStore;
            _userDataProtector = userDataProtector;
        }

        public async Task Save(
            ServiceBusConnection serviceBusConnection,
            CancellationToken cancellationToken)
        {
            ServiceBusSettings settings = await GetSettingsAsync(cancellationToken);
        }

        public async Task<ServiceBusSettings> GetSettingsAsync(CancellationToken cancellationToken)
        {
            ServiceBusSettings? settings =
                await _settingsStore.GetProtectedAsync<ServiceBusSettings>(
                    ServiceBusSettingsFileName,
                    cancellationToken: cancellationToken);

            if(settings == null)
            {
                settings = new ServiceBusSettings();
            }

            return settings;
        }
    }
}
