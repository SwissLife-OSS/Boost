using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;

namespace Boost
{
    public class Startup
    {
        public Startup()
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCookiePolicy();

            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHub<ConsoleHub>("signal");
            });

            app.UseEmbeddedUI("UI");
        }
    }
}
