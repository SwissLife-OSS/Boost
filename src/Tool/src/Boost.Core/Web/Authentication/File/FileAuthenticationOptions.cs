using Microsoft.AspNetCore.Authentication;

namespace Boost.Web.Authentication
{
    public class FileAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string Filename { get; set; } = default!;

        public bool StoreTokens { get; set; } = false;
    }
}
