using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Boost.Security;

public class AuthenticationSessionService : IAuthenticationSessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenAnalyzer _tokenAnalyzer;
    private readonly IIdentityService _identityService;

    public AuthenticationSessionService(
        IHttpContextAccessor httpContextAccessor,
        ITokenAnalyzer tokenAnalyzer,
        IIdentityService identityService)
    {
        _httpContextAccessor = httpContextAccessor;
        _tokenAnalyzer = tokenAnalyzer;
        _identityService = identityService;
    }

    public async Task<AuthenticationSessionInfo> GetSessionInfoAsync(
        CancellationToken cancellationToken)
    {
        AuthenticateResult? authResult = await _httpContextAccessor.HttpContext?
            .AuthenticateAsync()!;

        var session = new AuthenticationSessionInfo()
        {
            IsAuthenticated = authResult?.Principal?.Identity?.IsAuthenticated is true
        };

        if (session.IsAuthenticated)
        {
            session.Properties = authResult?.Properties?.Items;

            var accessToken = GetToken(session.Properties, "access");
            var idToken = GetToken(session.Properties, "id");
            session.RefreshToken = GetToken(session.Properties, "refresh");

            if (accessToken is { })
            {
                session.AccessToken = _tokenAnalyzer.Analyze(accessToken);

                session.UserInfo = await _identityService.GetUserInfoAsync(
                    accessToken,
                    cancellationToken);

            }
            if (idToken is { })
            {
                session.IdToken = _tokenAnalyzer.Analyze(idToken);
            }
        }

        return session;
    }

    private string? GetToken(IDictionary<string, string?>? data, string name)
    {
        if (data is { } && data.ContainsKey($".Token.{name}_token"))
        {
            return data[$".Token.{name}_token"];
        }

        return null;
    }
}
