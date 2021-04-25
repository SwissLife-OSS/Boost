using Boost.Security;

namespace Boost.GraphQL
{
    public record GetStoredTokenInput(string Id, TokenType Type, bool AutoRefresh);
}
