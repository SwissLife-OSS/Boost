using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.GraphQL;
using Boost.Nuget;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.Core.GraphQL
{
    [ExtendObjectType(RootTypes.Query)]
    public class NugetQueries
    {
        public Task<NugetPackageVersionInfo> GetNugetPackageVersionsAsync(
            [Service] INugetService nugetService,
            CancellationToken cancellationToken)
        {
            return nugetService.GetLatestVersionAsync("Azure.Identity", cancellationToken);
        }
    }
}
