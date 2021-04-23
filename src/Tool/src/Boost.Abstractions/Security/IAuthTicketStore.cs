using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Security
{
    public interface IAuthTokenStore
    {
        Task StoreAsync(TokenStoreModel model, CancellationToken cancellationToken);

        Task<TokenStoreModel> GetAsync(string name, CancellationToken cancellationToken);
    }

    public record TokenStoreModel(string Name, DateTime CreatedAt)
    {
        public IList<TokenInfo> Tokens { get; init; } = new List<TokenInfo>();
    }

    public record TokenInfo(TokenType Type, string Token)
    {
        public DateTimeOffset? ExpiresAt { get; init; }
    }

    public enum TokenType
    {
        Access,
        Id,
        Refresh
    }
}