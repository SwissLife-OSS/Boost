using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Nuget;
using Semver;

namespace Boost.Infrastructure
{
    public class VersionChecker : IVersionChecker
    {
        private readonly INugetService _nugetService;
        private readonly AppSettings _appSettings;
        private readonly IBoostApplicationContext _boostApplicationContext;

        public VersionChecker(
            INugetService nugetService,
            AppSettings appSettings,
            IBoostApplicationContext boostApplicationContext)
        {
            _nugetService = nugetService;
            _appSettings = appSettings;
            _boostApplicationContext = boostApplicationContext;
        }

        public async Task<BoostVersionInfo> GetVersionInfo(CancellationToken cancellationToken)
        {
            NugetPackageInfo? version = await _nugetService
                .GetNugetPackageInfoAsync(_appSettings.PackageId, cancellationToken);

            var versionInfo = new BoostVersionInfo
            {
                Installed = _boostApplicationContext.Version,
                PackageId = _appSettings.PackageId
            };

            if (version is { })
            {
                versionInfo.Latest = version.LatestStable;
                versionInfo.PreRelease = version.LatestPreRelease;

                if (versionInfo.PreRelease is { } &&
                    versionInfo.Latest is { } &&
                    versionInfo.Latest.ToSemVersion() > versionInfo.PreRelease.ToSemVersion())
                {
                    versionInfo.PreRelease = null;
                }

                var installed = SemVersion.Parse(versionInfo.Installed);

                if (versionInfo.Latest is { })
                {
                    versionInfo.NewerAvailable = SemVersion.Parse(versionInfo.Latest.Version) > installed;
                }

                if (versionInfo.PreRelease is { })
                {
                    versionInfo.NewerPreReleaseAvailable = SemVersion.Parse(versionInfo.PreRelease.Version) > installed;
                }
            }

            return versionInfo;
        }

    }
}
