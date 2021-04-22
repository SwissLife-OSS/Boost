using System.Collections.Generic;
using Boost.Security;

namespace Boost.GraphQL
{
    public record TokenRequestInput(
        string Authority,
        string ClientId,
        string Secret,
        string GrantType,
        IEnumerable<string> Scopes,
        IEnumerable<TokenRequestParameter> Parameters);
}
