using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;

namespace Boost.Security;

public class UserDataAuthTokenStoreReader : IAuthTokenStoreReader
{
    private readonly IAuthTokenStore _authTokenStore;
    private readonly IIdentityRequestStore _identityRequestStore;
    private readonly IIdentityService _identityService;

    public UserDataAuthTokenStoreReader(
        IAuthTokenStore authTokenStore,
        IIdentityRequestStore identityRequestStore,
        IIdentityService identityService)
    {
        _authTokenStore = authTokenStore;
        _identityRequestStore = identityRequestStore;
        _identityService = identityService;
    }

    public async Task<IEnumerable<TokenStoreHeader>> GetTokensAsync(
        CancellationToken cancellationToken)
    {
        var path = SettingsStore.GetUserDirectory("auth_token");
        var entries = new List<TokenStoreHeader>();

        foreach (FileInfo? file in new DirectoryInfo(path).GetFiles())
        {
            TokenStoreModel? authData = await _authTokenStore.GetAsync(
                file.Name,
                cancellationToken);

            if (authData is null)
            {
                continue;
            }

            IdentityRequestItem? request = await TryGetIdentityRequest(
                authData,
                cancellationToken);

            TokenStoreHeader entry = ToTokenHeader(authData, request);

            entries.Add(entry);
        }

        return entries;
    }

    private static TokenStoreHeader ToTokenHeader(
        TokenStoreModel authData,
        IdentityRequestItem? request)
    {
        var header = new TokenStoreHeader(
            authData.Name,
            request?.Name ?? authData.Name,
            authData.CreatedAt);

        if (request is { })
        {
            header.RequestId = request.Id;
        }

        TokenInfo? accessToken = authData.Tokens.FirstOrDefault(
            x => x.Type == TokenType.Access);

        if (accessToken is { })
        {
            header.HasAccessToken = true;
            header.AccessTokenExpiresIn =
                (int)(accessToken.ExpiresAt!.Value - DateTimeOffset.UtcNow).TotalMinutes;
        }

        header.HasIdToken = authData.Tokens.Any(x => x.Type == TokenType.Id);
        header.HasRefreshToken = authData.Tokens.Any(x => x.Type == TokenType.Refresh);

        return header;
    }

    private async Task<IdentityRequestItem?> TryGetIdentityRequest(
        TokenStoreModel authData,
        CancellationToken cancellationToken)
    {
        if (authData.Name.StartsWith("R_"))
        {
            if (Guid.TryParse(authData.Name.Split("_").Last(), out Guid parsed))
            {
                return await _identityRequestStore.GetByIdAsync(parsed, cancellationToken);
            }
        }

        return null;
    }

    public async Task<TokenStoreHeader> RefreshAsync(
        string id,
        CancellationToken cancellationToken)
    {
        TokenStoreModel? authData = await _authTokenStore.GetAsync(
            id,
            cancellationToken);

        TokenStoreModel refreshed = await RefreshAsync(authData, cancellationToken);

        IdentityRequestItem? request = await TryGetIdentityRequest(
            authData,
            cancellationToken);

        return ToTokenHeader(refreshed, request);
    }

    private async Task<TokenStoreModel> RefreshAsync(
        TokenStoreModel authData,
        CancellationToken cancellationToken)
    {
        TokenInfo? refreshToken = authData.Tokens.SingleOrDefault(
                x => x.Type == TokenType.Refresh);

        if (refreshToken is null)
        {
            throw new ApplicationException(
              "No refresh token found");
        }

        IdentityRequestItem? request = await TryGetIdentityRequest(
            authData,
            cancellationToken);

        if (request is null)
        {
            throw new ApplicationException(
                "No request assosiated with this token, " +
                "can not refresh");
        }

        IEnumerable<TokenInfo> refreshedTokens = await _identityService
            .RefreshTokenAsync(
                request.Data!,
                refreshToken.Token,
                cancellationToken);

        authData = authData with { Tokens = refreshedTokens.ToList() };

        await _authTokenStore.StoreAsync(authData, cancellationToken);

        return authData;
    }

    public async Task<string?> GetTokenAsync(
        string id,
        TokenType tokenType,
        bool autoRefresh,
        CancellationToken cancellationToken)
    {
        TokenStoreModel? authData = await _authTokenStore.GetAsync(
            id,
            cancellationToken);

        if (authData is { })
        {
            TokenInfo? token = authData.Tokens.SingleOrDefault(x => x.Type == tokenType);

            if (token is { })
            {
                if (token.Type == TokenType.Access)
                {
                    if (token.ExpiresAt < DateTime.UtcNow.AddMinutes(-5) && autoRefresh)
                    {
                        TokenStoreModel? refreshedData = await RefreshAsync(
                            authData,
                            cancellationToken);

                        return refreshedData.Tokens.FirstOrDefault(
                            x => x.Type == TokenType.Access)?.Token;
                    }
                    else
                    {
                        return token.Token;
                    }
                }
                else
                {
                    return authData.Tokens.FirstOrDefault(
                        x => x.Type == tokenType)?.Token;
                }
            }
        }

        return null;
    }
}
