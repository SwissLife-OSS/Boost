using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Boost.Nuget
{
    public class NugetService : INugetService
    {
        private readonly NugetPackageSourceFactory _sourceFactory;
        private readonly string DefaultSource = "f2c-main-core";

        public NugetService(NugetPackageSourceFactory sourceFactory)
        {
            _sourceFactory = sourceFactory;
        }

        public ILogger NugetLogger => NullLogger.Instance;

        public SourceCacheContext Cache => new SourceCacheContext();

        public async Task<FindPackageByIdResource> GetResourceAsync(
            string sourceName,
            CancellationToken cancellationToken)
        {
            sourceName = sourceName ?? DefaultSource;
            PackageSource? source = await _sourceFactory.CreateAsync(
                sourceName,
                cancellationToken);

            SourceRepository repository = Repository.Factory.GetCoreV3(source);
            FindPackageByIdResource resource = await repository
                .GetResourceAsync<FindPackageByIdResource>();

            return resource;
        }

        public async Task<NugetPackageInfo> GetNugetPackageInfo(
            string? sourceName,
            string packageId,
            CancellationToken cancellationToken)
        {
            sourceName = sourceName ?? DefaultSource;
            PackageSource? source = await _sourceFactory.CreateAsync(
                sourceName,
                cancellationToken);

            SourceRepository repository = Repository.Factory.GetCoreV3(source);

            PackageMetadataResource metaloader = await repository
                .GetResourceAsync<PackageMetadataResource>();

            IEnumerable<IPackageSearchMetadata> metadata = await metaloader
                .GetMetadataAsync(
                    packageId,
                    includePrerelease: true,
                    includeUnlisted: false,
                    Cache,
                    NugetLogger,
                    cancellationToken);

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

        public async Task<NugetPackageVersionInfo> GetLatestVersionAsync(
            string sourceName,
            string packageId,
            CancellationToken cancellationToken)
        {
            sourceName = sourceName ?? DefaultSource;
            PackageSource source = await _sourceFactory.CreateAsync(sourceName, cancellationToken);

            SourceRepository repository = Repository.Factory.GetCoreV3(source);

            PackageMetadataResource metaloader = await repository.GetResourceAsync<PackageMetadataResource>();
            IEnumerable<IPackageSearchMetadata> metadata = await metaloader.GetMetadataAsync(
                packageId,
                includePrerelease: true,
                includeUnlisted: false,
                Cache,
                NugetLogger,
                cancellationToken);

            IPackageSearchMetadata latest = metadata.OrderBy(x => x.Published).LastOrDefault();

            return CreateVersionInfo(latest);
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
