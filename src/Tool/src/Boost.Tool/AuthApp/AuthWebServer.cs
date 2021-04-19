using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.GraphQL;
using Boost.Security;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Boost.Tool.AuthApp
{
    public class AuthWebServer : IAuthWebServer
    {
        Dictionary<RunningWebServerInfo, IHost> _hosts
            = new Dictionary<RunningWebServerInfo, IHost>();

        public async Task<RunningWebServerInfo> StartAsync(
            StartWebServerOptions serverOptions,
            CancellationToken cancellationToken)
        {
            var url = $"http://localhost:{serverOptions.Port}";

            IHost _host = Host.CreateDefaultBuilder()
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(url);
                    webBuilder.UseStartup<AuthStartup>();
                })
                .ConfigureServices((ctx, services) =>
                {
                    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                    serverOptions.SetupAction?.Invoke(services);

                    services.AddControllers();

                    services.AddGraphQLServer()
                          .AddQueryType(d => d.Name(RootTypes.Query))
                          .AddType<AuthQueries>();

                    services.AddSingleton<
                        IAuthenticationSessionService,
                        AuthenticationSessionService>();
                    services.AddSingleton<ITokenAnalyzer, TokenAnalyzer>();
                    services.AddSingleton<IIdentityService, IdentityService>();

                    services.AddHttpContextAccessor();
                    services.AddSameSiteOptions();
                    services.AddOptions<OpenIdConnectOptions>("oidc")
                      .Configure<AuthorizeRequestData>((options, authData) =>
                      {
                          options.Authority = authData.Authority;
                          options.ClientSecret = authData.Secret;
                          options.ClientId = authData.ClienId;

                          options.Scope.Clear();
                          foreach (string scope in authData.Scopes)
                          {
                              options.Scope.Add(scope);
                          }
                      });

                    services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = "oidc";
                    })
                    .AddCookie(options =>
                    {
                        options.ExpireTimeSpan = TimeSpan.FromDays(30);
                        options.Cookie.Name = $".ba.{serverOptions.Id.ToString("N").Substring(0, 8)}";
                    })
                    .AddOpenIdConnect("oidc", options =>
                    {
                        options.ResponseType = "code";
                        options.SaveTokens = true;
                        options.GetClaimsFromUserInfoEndpoint = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = JwtClaimTypes.Name,
                            RoleClaimType = JwtClaimTypes.Role, 
                        };
                    });

                    services.AddHttpClient();

                })

                .Build();

            await _host.StartAsync(cancellationToken);

            RunningWebServerInfo server = new RunningWebServerInfo(serverOptions.Id, url)
            {
                Title = serverOptions.Title
            };

            _hosts.Add(server, _host);

            return server;
        }

        public IEnumerable<RunningWebServerInfo> GetRunningServers()
        {
            return _hosts.Keys;
        }

        public async Task StopAsync(Guid id, CancellationToken cancellationToken)
        {
            RunningWebServerInfo? key = _hosts.Keys.SingleOrDefault(x => x.Id == id);

            if (key is { })
            {
                await _hosts[key].StopAsync(cancellationToken);
            }
        }
    }
}