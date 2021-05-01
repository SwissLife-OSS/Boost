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
        public Task<NugetPackageInfo?> GetNugetPackageVersionsAsync(
            [Service] INugetService nugetService,
            string packageId,
            CancellationToken cancellationToken)
        {
            return nugetService.GetNugetPackageInfoAsync(packageId, cancellationToken);
        }
    }
}
