using Boost.GitHub.WebAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.GitHub;

public static class AuthenticationExtensions
{
    public static AuthenticationBuilder AddGitHub(
        this AuthenticationBuilder builder,
        GitHubAuthContext authContext)
    {
        builder.AddGitHub("GitHub", options =>
         {
             options.ClientId = authContext.OAuth.ClientId;
             options.ClientSecret = authContext.OAuth.Secret;
             options.Scope.Add("repo");
             options.Scope.Add("read:user");
             options.Events = new OAuthEvents
             {
                 OnTicketReceived = async (ctx) =>
                 {
                     ctx.HandleResponse();
                     await ctx.Response.WriteAsync("Service authorized!");
                 },
                 OnCreatingTicket = async (ctx) =>
                 {
                     IOAuthTicketHandler? handler = ctx.HttpContext.RequestServices
                         .GetRequiredService<IOAuthTicketHandler>();

                     await handler.ProcessTicketAsync(ctx);
                 },
             };
         });

        return builder;
    }
}
