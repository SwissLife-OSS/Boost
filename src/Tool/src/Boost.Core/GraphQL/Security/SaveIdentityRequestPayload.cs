using Boost.Security;

namespace Boost.GraphQL;

public class SaveIdentityRequestPayload
{
    public SaveIdentityRequestPayload(IdentityRequestItem item)
    {
        Item = item;
    }

    public IdentityRequestItem Item { get; }
}
