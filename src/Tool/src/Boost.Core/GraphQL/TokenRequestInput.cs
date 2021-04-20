using System.Collections.Generic;

namespace Boost.GraphQL
{
    public record TokenRequestInput(
        string Authority,
        string ClientId,
        string Secret,
        string GrantType,
        IEnumerable<string> Scopes);
}
