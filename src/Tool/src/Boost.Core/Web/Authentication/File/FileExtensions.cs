using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.Web.Authentication
{
    public static class FileExtensions
    {
        public static AuthenticationBuilder AddFile(
            this AuthenticationBuilder builder,
            Action<FileAuthenticationOptions> configure)
        {
            builder.Services.AddSingleton<IDataSerializer<AuthenticationTicket>, TicketSerializer>();

            return builder.AddScheme<FileAuthenticationOptions, FileAuthenticationHandler>(
                FileAuthenticationDefaults.AuthenticationScheme, "File", configure);
        }
    }
}
