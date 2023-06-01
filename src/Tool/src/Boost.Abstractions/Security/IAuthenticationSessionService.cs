using System.Threading;
using System.Threading.Tasks;

namespace Boost.Security;

public interface IAuthenticationSessionService
{
    Task<AuthenticationSessionInfo> GetSessionInfoAsync(CancellationToken cancellationToken);
}