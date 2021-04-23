using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;

namespace Boost.Security
{
    public class UserDataAuthTokenStoreReader : IUserDataAuthTokenStoreReader
    {
        private readonly IAuthTokenStore _authTokenStore;
        private readonly IIdentityRequestStore _identityRequestStore;

        public UserDataAuthTokenStoreReader(
            IAuthTokenStore authTokenStore,
            IIdentityRequestStore identityRequestStore)
        {
            _authTokenStore = authTokenStore;
            _identityRequestStore = identityRequestStore;
        }

        public async Task<IEnumerable<TokenStoreHeader>> GetTokensAsync(CancellationToken cancellationToken)
        {
            var path = SettingsStore.GetUserDirectory("auth_token");
            var entries = new List<TokenStoreHeader>();

            foreach (FileInfo? file in new DirectoryInfo(path).GetFiles())
            {

                TokenStoreModel? authData = await _authTokenStore.GetAsync(file.Name, cancellationToken);

                if (authData is null)
                {
                    continue;
                }

                IdentityRequestItem? request = null;

                if (authData.Name.StartsWith("R-"))
                {
                    if (Guid.TryParse(authData.Name.Split("-").Last(), out Guid parsed))
                    {
                        request = await _identityRequestStore.GetByIdAsync(parsed, cancellationToken);
                    }
                }

                var entry = new TokenStoreHeader(request?.Name ?? authData.Name, authData.CreatedAt);
                if (request is { })
                {
                    entry.RequestId = request.Id;
                }

                TokenInfo? accessToken = authData.Tokens.FirstOrDefault(x => x.Type == TokenType.Access);
                if (accessToken is { })
                {
                    entry.HasAccessToken = true;
                    entry.AccessTokensExpiresAt = accessToken.ExpiresAt;
                }

                entry.HasIdToken = authData.Tokens.Any(x => x.Type == TokenType.Id);
                entry.HasRefreshToken = authData.Tokens.Any(x => x.Type == TokenType.Refresh);

                entries.Add(entry);
            }

            return entries;
        }
    }
}
