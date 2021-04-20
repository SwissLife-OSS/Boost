using System.Collections.Generic;

namespace Boost.GraphQL
{
    public record AuthorizeRequestInput(
        string Authority,
        string ClientId,
        string Secret,
        IEnumerable<string> Scopes,
        bool Pkce)
    {
        public int Port { get; init; }
    }
}
