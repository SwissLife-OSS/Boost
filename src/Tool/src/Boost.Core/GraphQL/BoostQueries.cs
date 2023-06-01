using System.Threading;
using System.Threading.Tasks;
using Boost.GraphQL;
using Boost.Infrastructure;
using Boost.Navigation;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.Core.GraphQL;

[ExtendObjectType(RootTypes.Query)]
public class BoostQueries
{
    private readonly IBoostApplicationContext _boostApplicationContext;

    public BoostQueries(IBoostApplicationContext boostApplicationContext)
    {
        _boostApplicationContext = boostApplicationContext;
    }

    public BoostApplication GetAppliation()
    {
        var app = new BoostApplication
        {
            WorkingDirectory = _boostApplicationContext.WorkingDirectory.FullName,
            Version = _boostApplicationContext.Version,
            ConfigurationRequired = true,
        };

        return app;
    }

    public Task<BoostVersionInfo> GetVersionAsync(
        [Service] IVersionChecker versionChecker,
        CancellationToken cancellationToken)
    {
        return versionChecker.GetVersionInfo(cancellationToken);
    }

    public AppNavigation GetAppNavigation(
        [Service] IAppNavigationService appNavigationService)
    {
        return appNavigationService.GetNavigation();
    }
}
