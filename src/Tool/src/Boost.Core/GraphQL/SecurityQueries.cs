using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Security;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.GraphQL
{
    [ExtendObjectType(Name = RootTypes.Query)]
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
    }
}
