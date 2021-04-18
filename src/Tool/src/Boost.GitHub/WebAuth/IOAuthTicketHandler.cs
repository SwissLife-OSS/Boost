using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace Boost.GitHub.WebAuth
{
    public interface IOAuthTicketHandler
    {
        Task ProcessTicketAsync(OAuthCreatingTicketContext ctx);
    }
}