using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boost.Security
{
    public interface IAuthTokenStore
    {
        Task StoreAsync(TokenStoreModel model);

        Task GetAsync(TokenStoreModel model);

        Task RemoveAsync(TokenStoreModel model);
    }

    public record TokenStoreModel(string Name)
    {
        public IList<TokenInfo> Tokens { get; init; } = new List<TokenInfo>();
    }

    public record TokenInfo(string Type, string Token)
    {
        public DateTime? ExpiresAt { get; init; }
    }
}
