using Boost.Web;
using Boost.Web.Authentication;
using Boost.WebApp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;

namespace Boost.AuthApp
{
    public class AuthStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseCookiePolicy();
            app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthentication();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/logout"))
                {
                    await context.SignOutAsync(FileAuthenticationDefaults.AuthenticationScheme);
                    await context.SignOutAsync("oidc");
                    return;
                }

                if (context.Request.Path.StartsWithSegments("/graphql"))
                {
                    await next();
                    return;
                }

                if ( !context.User.Identity?.IsAuthenticated is true)
                {
                    await context.ChallengeAsync("oidc");
                }
                else
                {
                    await next();
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapGraphQL();
            });

            app.UseEmbeddedUI("AuthUI");
        }
    }
}
