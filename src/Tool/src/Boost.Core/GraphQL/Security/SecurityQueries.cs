using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Security;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.GraphQL;

[ExtendObjectType(RootTypes.Query)]
public class SecurityQueries
{
    public TokenModel? AnalyzeToken(
        [Service] ITokenAnalyzer analyzer,
        string token)
    {
        return analyzer.Analyze(token);
    }

    public Task<UserInfoResult> GetUserInfoClaimsAsync(
        string token,
        [Service] IIdentityService identityService,
        CancellationToken cancellationToken)
    {
        return identityService.GetUserInfoAsync(token, cancellationToken);
    }

    public IEnumerable<RunningWebServerInfo> GetRunningAuthServers(
        [Service] IAuthWebServer authWebServer)
    {
        return authWebServer.GetRunningServers();
    }

    public Task<IEnumerable<IdentityRequestItem>> SearchIdentityRequestsAsync(
        SearchIdentityRequest input,
        [Service] IIdentityRequestStore store,
        CancellationToken cancellationToken)
    {
        return store.SearchAsync(input, cancellationToken);
    }

    public Task<IdentityRequestItem> GetIdentityRequestAsync(
        Guid id,
        [Service] IIdentityRequestStore store,
        CancellationToken cancellationToken)
    {
        return store.GetByIdAsync(id, cancellationToken);
    }

    public Task<IEnumerable<TokenStoreHeader>> GetStoredTokensAsync(
        [Service] IAuthTokenStoreReader tokenReader,
        CancellationToken cancellationToken)
    {
        return tokenReader.GetTokensAsync(cancellationToken);
    }

    public Task<string?> GetStoredTokenAsync(
        [Service] IAuthTokenStoreReader tokenReader,
        GetStoredTokenInput input,
        CancellationToken cancellationToken)
    {
        return tokenReader.GetTokenAsync(
            input.Id,
            input.Type,
            input.AutoRefresh,
            cancellationToken);
    }
}
