using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semver;

namespace Boost.Nuget
{
    public static class NugetPackageExtensions
    {
        public static SemVersion ToSemVersion(this NugetPackageVersionInfo versionInfo)
        {
            return SemVersion.Parse(versionInfo.Version);
        }
    }
}
