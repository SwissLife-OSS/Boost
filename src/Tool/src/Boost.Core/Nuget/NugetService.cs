using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Infrastructure;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace Boost.Nuget
{
    public class NugetService : INugetService
    {
        //https://gist.github.com/alistairjevans/4de1dccfb7288e0460b7b04f9a700a04

        private readonly IBoostApplicationContext _boostApplicationContext;

        public NugetService(
            IBoostApplicationContext boostApplicationContext)
        {
            _boostApplicationContext = boostApplicationContext;
        }

        private NuGet.Common.ILogger NugetLogger => new SerilogNugetLogger();

        private SourceCacheContext Cache => new SourceCacheContext();

        public async Task<NugetPackageInfo?> GetNugetPackageInfoAsync(
              string packageId,
              CancellationToken cancellationToken)
        {
            IEnumerable<SourceRepository> repositories = BuildRepositories();

            foreach (SourceRepository? repo in repositories)
            {
                NugetPackageInfo? pgkInfo = await GetNugetPackageInfoAsync(
                    repo,
                    packageId,
                    cancellationToken);

                if (pgkInfo is { })
                {
                    return pgkInfo;
                }
            }

            return null;
        }

        private IEnumerable<SourceRepository> BuildRepositories()
        {
            ISettings settings = NuGet.Configuration.Settings.LoadDefaultSettings(
                _boostApplicationContext.WorkingDirectory.FullName);

            var packageSourceProvider = new PackageSourceProvider(settings);

            var sourceRepositoryProvider = new SourceRepositoryProvider(
                packageSourceProvider,
                Repository.Provider.GetCoreV3());

            IEnumerable<SourceRepository>? repositories = sourceRepositoryProvider.GetRepositories();
            return repositories;
        }

        private async Task<NugetPackageInfo?> GetNugetPackageInfoAsync(
            SourceRepository repository,
            string packageId,
            CancellationToken cancellationToken)
        {
            PackageMetadataResource metaloader = await GetResourceAsync<PackageMetadataResource>(
                repository,
                cancellationToken);

            IEnumerable<IPackageSearchMetadata> metadata = await metaloader
                .GetMetadataAsync(
                    packageId,
                    includePrerelease: true,
                    includeUnlisted: false,
                    Cache,
                    NugetLogger,
                    cancellationToken);

            if (!metadata.Any())
            {
                return null;
            }

            IPackageSearchMetadata? latestStable = metadata
                .Where(x => !x.Identity.Version.IsPrerelease)
                .OrderBy(x => x.Published).LastOrDefault();

            IPackageSearchMetadata? latestPrerelease = metadata
                .Where(x => x.Identity.Version.IsPrerelease)
                .OrderBy(x => x.Published).LastOrDefault();

            return new NugetPackageInfo(packageId)
            {
                LatestPreRelease = CreateVersionInfo(latestPrerelease),
                LatestStable = CreateVersionInfo(latestStable)
            };
        }

        private async Task<NResource> GetResourceAsync<NResource>(
            SourceRepository repository,
            CancellationToken cancellationToken)
                where NResource : class, INuGetResource
        {
            NResource resource = await repository
                .GetResourceAsync<NResource>(cancellationToken);

            return resource;
        }

        private NugetPackageVersionInfo? CreateVersionInfo(IPackageSearchMetadata? metadata)
        {
            if (metadata is null)
            {
                return null;
            }

            return new NugetPackageVersionInfo(
                metadata.Identity.Version.OriginalVersion,
                metadata.Published.Value);
        }
    }
}
