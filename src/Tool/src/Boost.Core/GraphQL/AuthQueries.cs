using System.Threading;
using System.Threading.Tasks;
using Boost.Security;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.GraphQL
{
    [ExtendObjectType(Name = RootTypes.Query)]
    public class AuthQueries
    {
        public Task<AuthenticationSessionInfo> GetAuthenticationSessionAsync(
            [Service] IAuthenticationSessionService authenticationSession,
            CancellationToken cancellationToken)
        {
            return authenticationSession.GetSessionInfoAsync(cancellationToken);
        }
    }
}
