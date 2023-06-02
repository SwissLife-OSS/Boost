using Boost.Security;

namespace Boost.GraphQL;

public class RefreshStoredTokenPayload
{

    public RefreshStoredTokenPayload(TokenStoreHeader header)
    {
        Header = header;
    }

    public TokenStoreHeader Header { get; }
}
