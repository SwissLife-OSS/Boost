using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Security
{
    public interface IAuthTokenStoreReader
    {
        Task<string?> GetTokenAsync(string id, TokenType tokenType, bool authRefresh, CancellationToken cancellationToken);
        Task<IEnumerable<TokenStoreHeader>> GetTokensAsync(CancellationToken cancellationToken);
        Task<TokenStoreHeader> RefreshAsync(string id, CancellationToken cancellationToken);
    }
}