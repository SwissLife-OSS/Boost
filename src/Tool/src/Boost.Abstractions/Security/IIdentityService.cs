using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Security
{
    public interface IIdentityService
    {
        Task<UserInfoResult> GetUserInfoAsync(
            string token,
            CancellationToken cancellationToken);
    }

    public record UserInfoResult(string? Error)
    {
        public IEnumerable<UserClaim> Claims { get; init; } = Array.Empty<UserClaim>();
    }

    public record UserClaim(string Type, string Value);

}
