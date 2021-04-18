using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.GitHub
{
    public static class SameSiteCookieExtensions
    {
        private static void CheckSameSite(CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                options.SameSite = SameSiteMode.Unspecified;
            }
        }

        public static IServiceCollection AddSameSiteOptions(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext =>
                    CheckSameSite(cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                    CheckSameSite(cookieContext.CookieOptions);
            });

            return services;
        }
    }
}
