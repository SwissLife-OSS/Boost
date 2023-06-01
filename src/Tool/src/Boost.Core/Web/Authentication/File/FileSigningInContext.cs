using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Boost.Web.Authentication;

public class FileSigningInContext : PrincipalContext<FileAuthenticationOptions>
{
    public FileSigningInContext(
        HttpContext context,
        AuthenticationScheme scheme,
        FileAuthenticationOptions options,
        ClaimsPrincipal principal,
        AuthenticationProperties? properties)
            : base(context, scheme, options, properties)
    {
        Principal = principal;
    }
}
