using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Boost.Account;
using Boost.Infrastructure;

namespace Boost
{
    public static class AuthenticationExtensions
    {
        public static AuthenticationBuilder AddAuthentication(
            this IServiceCollection services)
        {
            return services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.Cookie.Name = $"boost-id";
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "";
                options.ClientSecret = "";
                options.ClientId = "";
                options.ResponseType = "code";

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("offline_access");

                options.ClaimActions.MapAllExcept("iss", "nbf", "exp", "aud", "nonce", "iat", "c_hash");
                options.SaveTokens = false;
                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = (ctx) =>
                    {
                        return Task.CompletedTask;
                    },
                    OnAuthorizationCodeReceived = async (ctx) =>
                    {
                        ICredentialStore credStore = ctx.HttpContext.RequestServices
                            .GetRequiredService<ICredentialStore>();

                        HttpClient httpClient = ctx.HttpContext.RequestServices
                            .GetRequiredService<IHttpClientFactory>()
                            .CreateClient();

                        TokenResponse response = await httpClient.RequestAuthorizationCodeTokenAsync(
                            new AuthorizationCodeTokenRequest
                            {
                                Address = options.Authority.Trim('/') + "/connect/token",
                                ClientId = options.ClientId,
                                ClientSecret = options.ClientSecret,
                                Code = ctx.ProtocolMessage.Code,
                                RedirectUri = ctx.TokenEndpointRequest.RedirectUri,
                                CodeVerifier = ctx.TokenEndpointRequest.Parameters["code_verifier"]
                            });

                        await credStore.StoreAsync(
                            CredentialNames.BoostAccessToken,
                            response.AccessToken,
                            global: false,
                            default);

                        await credStore.StoreAsync(
                            CredentialNames.BoostRefreshToken,
                            response.RefreshToken,
                            global: false,
                            default);

                        ctx.HandleCodeRedemption(response.AccessToken, response.IdentityToken);
                    },

                    OnTicketReceived = (ctx) =>
                    {
                        return Task.CompletedTask;
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,
                };
            });
        }
    }
}
