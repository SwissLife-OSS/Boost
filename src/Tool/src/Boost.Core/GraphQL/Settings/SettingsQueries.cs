using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.GraphQL;
using Boost.Security;
using Boost.Settings;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.Core.GraphQL
{
    [ExtendObjectType(RootTypes.Query)]
    public class SettingsQueries
    {
        public Task<UserSettings> GetUserSettingsAsync(
            [Service] IUserSettingsManager settingsManager,
            CancellationToken cancellationToken)
        {
            return settingsManager.GetAsync(cancellationToken);
        }

        public async Task<IEnumerable<ConnectedService>> GetConnectedServices(
            [Service] IConnectedServiceManager serviceManager,
            CancellationToken cancellationToken)
        {
            IEnumerable<IConnectedService> services = await serviceManager.GetServicesAsync(cancellationToken);

            return services.Select(x => new ConnectedService
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
            });
        }

        public async Task<ConnectedService?> GetConnectedServiceAsync(
            Guid id,
            [Service] IConnectedServiceManager serviceManager,
            CancellationToken cancellationToken)
        {
            ConnectedService? service = await serviceManager.GetServiceAsync(
                id,
                cancellationToken);

            return service;
        }

        public IEnumerable<ConnectedServiceType> GetConnectedServiceTypes(
            [Service] IConnectedServiceManager serviceManager)
        {
            return serviceManager.GetServiceTypes();
        }
    }
}
