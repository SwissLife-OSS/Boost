using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;

namespace Boost.GitHub
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseCookiePolicy();
            app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthentication();

            app.Run(async context =>
            {
               await context.ChallengeAsync("GitHub");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
