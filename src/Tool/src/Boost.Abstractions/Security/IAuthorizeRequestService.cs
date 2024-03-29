using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Security;

public interface IAuthorizeRequestService
{
    Task<RunningWebServerInfo> StartAuthorizeRequestAsync(AuthorizeRequestData request, CancellationToken cancellationToken);
}

public record AuthorizeRequestSession(Guid Id, string Url, int Port);

public record AuthorizeRequestData(
    string Authority,
    string ClientId,
    IEnumerable<string> ResponseTypes,
    string? Secret,
    IEnumerable<string> Scopes,
    bool UsePkce)
{
    public int? Port { get; init; }

    public bool SaveTokens { get; init; }

    public string? RequestId { get; init; }
}

public record TokenRequestData(
    string Authority,
    string ClientId,
    string Secret,
    string GrantType,
    IEnumerable<string> Scopes,
    IEnumerable<TokenRequestParameter> Parameters)
{
    public bool SaveTokens { get; init; }

    public Guid? RequestId { get; init; }
}


public record RequestTokenResult(bool IsSuccess)
{
    public TokenModel? AccessToken { get; set; }

    public string? ErrorMessage { get; set; }
}

public record TokenRequestParameter(string Name)
{
    public string? Value { get; set; }
}
