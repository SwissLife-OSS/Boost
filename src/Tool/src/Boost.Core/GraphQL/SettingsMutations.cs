using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.GraphQL;
using Boost.Settings;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.Core.GraphQL
{
    [ExtendObjectType(Name = RootTypes.Mutation)]
    public class SettingsMutations
    {
        public async Task<SaveConnectedServicePayload> SaveConnectedServiceAsync(
            [Service] IConnectedServiceManager serviceManager,
            SaveConnectedServiceInput input,
            CancellationToken cancellationToken)
        {
            var service = new ConnectedService
            {
                Id = input.id ?? Guid.NewGuid(),
                Name = input.Name,
                Type = input.Type,
                DefaultWorkRoot = input.DefaultWorkRoot,
                Properties = input.Properties.ToList()
            };

            service = await serviceManager.SaveAsync(service, cancellationToken);

            return new SaveConnectedServicePayload(service);
        }

        public async Task<bool> SaveWorkRootsAsync(
            [Service] IUserSettingsManager settingsManager,
            SaveWorkRootsInput input,
            CancellationToken cancellationToken)
        {
            await settingsManager.SaveWorkRootsAsync(
                input.WorkRoots,
                cancellationToken);

            return true;
        }
    }

    public record SaveWorkRootsInput(IEnumerable<WorkRoot> WorkRoots);
}
