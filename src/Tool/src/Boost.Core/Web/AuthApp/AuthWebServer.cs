using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Certificates;
using Boost.Core.Settings;
using Boost.GraphQL;
using Boost.Infrastructure;
using Boost.Security;
using Boost.Web.Authentication;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Boost.AuthApp;

public class AuthWebServer : IAuthWebServer
{
    readonly Dictionary<RunningWebServerInfo, IHost> _hosts = new();

    public async Task<RunningWebServerInfo> StartAsync(
        StartWebServerOptions serverOptions,
        CancellationToken cancellationToken)
    {
        var startUrl = serverOptions.Port is null
            ? "http://*:0"
            : $"http://localhost:{serverOptions.Port}";

        IHost _host = Host.CreateDefaultBuilder()
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls(startUrl);
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
                services.AddSingleton<IAuthTokenStore, UserDataAuthTokenStore>();
                services.AddSingleton<ISettingsStore, SettingsStore>();

                services.AddSingleton<ICertificateManager, CertificateManager>();
                services.AddUserDataProtection();
                services.AddSingleton(c =>
                {
                    return c.GetRequiredService<IBoostCommandContext>()
                        .Services.GetRequiredService<IUserDataProtector>();
                });

                services.AddHttpContextAccessor();
                services.AddSameSiteOptions();
                services.AddOptions<OpenIdConnectOptions>("oidc")
                  .Configure<AuthorizeRequestData>((options, authData) =>
                  {
                      options.Authority = authData.Authority;
                      options.ClientSecret = authData.Secret;
                      options.ClientId = authData.ClientId;
                      options.ResponseType = string.Join(" ", authData.ResponseTypes);
                      options.UsePkce = authData.UsePkce;

                      options.Scope.Clear();
                      foreach (string scope in authData.Scopes)
                      {
                          options.Scope.Add(scope);
                      }
                  });

                services.AddOptions<FileAuthenticationOptions>(
                    FileAuthenticationDefaults.AuthenticationScheme)
                    .Configure<AuthorizeRequestData>((options, authData) =>
                    {
                        options.SaveTokens = authData.SaveTokens;
                        options.Filename = (authData.RequestId != null) ?
                            $"R-{authData.RequestId}" :
                            $"S-{serverOptions.Id.ToString("N").Substring(0, 8)}";
                    });

                services.AddAuthentication(options =>
                {
                    options.DefaultScheme = FileAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddFile()
                .AddOpenIdConnect("oidc", options =>
                {
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

        IServer server = _host.Services.GetRequiredService<IServer>();
        IServerAddressesFeature serverAddressesFeature = server.Features.Get<IServerAddressesFeature>()!;
        var port = new Uri(serverAddressesFeature.Addresses.Single()).Port;

        RunningWebServerInfo serverInfo = new RunningWebServerInfo(
            serverOptions.Id,
            $"http://localhost:{port}")
        {
            Title = serverOptions.Title
        };

        _hosts.Add(serverInfo, _host);

        return serverInfo;
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
