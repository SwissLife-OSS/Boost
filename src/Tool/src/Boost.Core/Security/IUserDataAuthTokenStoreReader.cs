using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Security
{
    public interface IUserDataAuthTokenStoreReader
    {
        Task<IEnumerable<TokenStoreHeader>> GetTokensAsync(CancellationToken cancellationToken);
    }
}