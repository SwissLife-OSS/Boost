using System.Threading;

namespace Boost.AzureDevOps;

public interface IAzureDevOpsPersonalAccessTokenResolver
{
    public string GetTokenAsync(CancellationToken cancellationToken);
}
