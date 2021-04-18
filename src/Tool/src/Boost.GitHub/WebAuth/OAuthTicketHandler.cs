using System.Linq;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.Settings;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.GitHub.WebAuth
{
    public class OAuthTicketHandler : IOAuthTicketHandler
    {
        public async Task ProcessTicketAsync(OAuthCreatingTicketContext ctx)
        {
            IConnectedServiceManager csm = ctx.HttpContext.RequestServices
                .GetRequiredService<IConnectedServiceManager>();

            GitHubAuthContext autContext = ctx.HttpContext.RequestServices
                .GetRequiredService<GitHubAuthContext>();

            ConnectedService? service = await csm.GetServiceAsync(
                autContext.Id,
                ctx.HttpContext.RequestAborted);

            ConnectedServiceProperty? accessToken = service.Properties
                .FirstOrDefault(x => x.Name == "AccessToken");

            if (accessToken is { })
            {
                service.Properties.Remove(accessToken);
            }

            service.Properties.Add(
                new ConnectedServiceProperty("AccessToken", ctx.AccessToken));

            await csm.SaveAsync(service, ctx.HttpContext.RequestAborted);
        }
    }
}
