using System.Threading;
using System.Threading.Tasks;

namespace Boost.Infrastructure;

public interface IVersionChecker
{
    Task<BoostVersionInfo> GetVersionInfo(CancellationToken cancellationToken);
}