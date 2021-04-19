using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Security
{
    public interface IAuthorizeRequestService
    {
        Task<RunningWebServerInfo> StartAuthorizeRequestAsync(AuthorizeRequestData request, CancellationToken cancellationToken);
    }

    public record AuthorizeRequestSession(Guid Id, string Url, int Port);

    public record AuthorizeRequestData(
        string Authority,
        string ClienId,
        string Secret,
        IEnumerable<string> Scopes,
        bool Pkce)
    {
        public int Port { get; init; } = 3010;
    }

    public record TokenRequestData(
        string Authority,
        string ClientId,
        string Secret,
        string GrantType,
        IEnumerable<string> Scopes);


    public record RequestTokenResult(bool IsSuccess)
    {
        public TokenModel? AccessToken { get; set; }

        public string? ErrorMessage { get; set; }
    };
}
