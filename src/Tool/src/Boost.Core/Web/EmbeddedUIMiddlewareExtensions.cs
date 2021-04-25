using Microsoft.AspNetCore.Builder;

namespace Boost.Web
{
    public static class EmbeddedUIMiddlewareExtensions
    {
        public static IApplicationBuilder UseEmbeddedUI(
            this IApplicationBuilder builder,
            string path)
        {
            return builder.UseMiddleware<EmbeddedUIMiddleware>(path);
        }
    }
}
