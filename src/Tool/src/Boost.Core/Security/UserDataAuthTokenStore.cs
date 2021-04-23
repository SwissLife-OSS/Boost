using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.Infrastructure;

namespace Boost.Security
{
    public class UserDataAuthTokenStore : IAuthTokenStore
    {
        private readonly ISettingsStore _settingsStore;

        public UserDataAuthTokenStore(ISettingsStore settingsStore)
        {
            _settingsStore = settingsStore;
        }

        public async Task<TokenStoreModel> GetAsync(string name, CancellationToken cancellationToken)
        {
            TokenStoreModel? tokenData = await _settingsStore.GetProtectedAsync<TokenStoreModel>(
                name,
                cancellationToken: cancellationToken);

            if (tokenData is null)
            {
                throw new ApplicationException($"No AuthData found with name: {name}");
            }

            return tokenData;
        }

        public async Task StoreAsync(TokenStoreModel model, CancellationToken cancellationToken)
        {
            await _settingsStore.SaveProtectedAsync(model, model.Name, cancellationToken: cancellationToken);
        }
    }
}
