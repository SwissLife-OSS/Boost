using Boost.Security;

namespace Boost.GraphQL
{
    public class RequestTokenPayload
    {
        public RequestTokenPayload(RequestTokenResult token)
        {
            Result = token;
        }

        public RequestTokenResult Result { get; }
    }
}
