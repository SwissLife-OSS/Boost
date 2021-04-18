using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Boost.Account;
using Boost.Infrastructure;
using IdentityModel.Client;

namespace Boost.Security
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentityService(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
    }
}
