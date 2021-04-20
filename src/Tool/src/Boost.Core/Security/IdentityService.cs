using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Boost.Security
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenAnalyzer _tokenAnalyzer;

        public IdentityService(
            IHttpClientFactory httpClientFactory,
            ITokenAnalyzer tokenAnalyzer)
        {
            _httpClientFactory = httpClientFactory;
            _tokenAnalyzer = tokenAnalyzer;
        }

        public async Task<UserInfoResult> GetUserInfoAsync(
            string token,
            CancellationToken cancellationToken)
        {
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken? jwt = handler.ReadToken(token) as JwtSecurityToken;

            if (jwt is { })
            {
                var issuer = jwt.Claims.Single(x => x.Type == "iss").Value;

                var client = new HttpClient();
                UserInfoResponse? response = await client.GetUserInfoAsync(
                    new UserInfoRequest
                    {
                        Address = issuer.Trim('/') + "/connect/userinfo",
                        Token = token
                    }, cancellationToken);

                return new UserInfoResult(response.Error)
                {
                    Claims = response.Claims.Select(x => new UserClaim(x.Type, x.Value))
                };
            }

            return new UserInfoResult("InvalidToken"); 
        }

        public async Task<RequestTokenResult> RequestTokenAsync(
            TokenRequestData request,
            CancellationToken cancellationToken)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            DiscoveryDocumentResponse disco = await GetDiscoveryDocumentAsync(request.Authority, cancellationToken);

            TokenResponse response = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = request.ClientId,
                ClientSecret = request.Secret,
                GrantType = request.GrantType,
                Scope = request.Scopes.Any() ? string.Join(" ", request.Scopes) : null
            });

            if (!response.IsError)
            {
                TokenModel? accessToken = _tokenAnalyzer.Analyze(response.AccessToken);

                return new RequestTokenResult(true)
                {
                    AccessToken = accessToken
                };
            }
            else
            {
                return new RequestTokenResult(false)
                {
                    ErrorMessage = response.Error
                };
            }
        }

        public async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(string authority, CancellationToken cancellationToken)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            DiscoveryDocumentResponse disco = await httpClient
                .GetDiscoveryDocumentAsync(authority, cancellationToken);

            return disco;
        }
    }
}
