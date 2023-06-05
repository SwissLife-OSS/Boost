using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.GitHub.WebAuth;
using Boost.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Boost.GitHub;

public class GitHubAuthServer : IGitHubAuthServer
{
    private readonly IConnectedServiceManager _connectedServiceManager;

    public GitHubAuthServer(IConnectedServiceManager connectedServiceManager)
    {
        _connectedServiceManager = connectedServiceManager;
    }

    public async Task StartAsync(
        int port,
        Guid id,
        Action<IServiceCollection> configure)
    {
        var url = $"http://localhost:{port}";
        GitHubAuthContext authContext = await GetAuthContext(id);

        IHost _host = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls(url);
                webBuilder.UseStartup<Startup>();
            }).ConfigureLogging(cf =>
            {
                if (!Debugger.IsAttached)
                {
                    cf.SetMinimumLevel(LogLevel.Warning);
                }
            }).ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddInMemoryCollection(
                    new Dictionary<string, string>
                    {
                        ["Boost:ServiceId"] = authContext.Id.ToString("N"),
                        ["Boost:OAuth:ClientId"] = authContext.OAuth.ClientId,
                        ["Boost:OAuth:Secret"] = authContext.OAuth.Secret,
                    });
            }).ConfigureServices((ctx, services) =>
           {
               JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
               configure(services);
               services.AddControllersWithViews();
               services.AddHttpContextAccessor();
               services.AddSameSiteOptions();
               services.AddSingleton<IOAuthTicketHandler, OAuthTicketHandler>();

               GitHubAuthContext authContext = ctx.Configuration.GetSection("Boost")
                   .Get<GitHubAuthContext>();

               services.AddSingleton(authContext);

               services.AddAuthentication(options =>
               {
                   options.DefaultChallengeScheme = "GitHub";
               }).AddGitHub(authContext);

               services.AddHttpClient();

           })

            .Build();

        Console.WriteLine($"GitHub auth server started on {url}");

        await _host.RunAsync();
    }

    private async Task<GitHubAuthContext> GetAuthContext(Guid id)
    {
        IConnectedService? service = await _connectedServiceManager.GetAsync(
            id,
            default);

        if (service is GitHubConnectedService gh)
        {
            return new GitHubAuthContext
            {
                Id = id,
                OAuth = gh.OAuth!
            };
        }

        throw new ApplicationException($"Could not get AuthContext from service id: {id}");
    }
}

public class GitHubAuthContext
{
    public Guid Id { get; set; }

    public GitHubOAuthConfig OAuth { get; set; } = default!;
}
