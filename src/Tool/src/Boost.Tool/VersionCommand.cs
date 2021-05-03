using System;
using System.Threading.Tasks;
using Boost.Infrastructure;
using Boost.Nuget;
using McMaster.Extensions.CommandLineUtils;
using Semver;

namespace Boost.Commands
{
    [Command(
        Name = "version",
        FullName = "Version",
        Description = "Get current Version and check for new version"), HelpOption]
    public class VersionCommand : CommandBase
    {
        private readonly INugetService _nugetService;
        private readonly IBoostApplicationContext _boostApplicationContext;

        public VersionCommand(
            INugetService nugetService,
            IBoostApplicationContext boostApplicationContext)
        {
            _nugetService = nugetService;
            _boostApplicationContext = boostApplicationContext;
        }

        public async Task OnExecute(
            CommandLineApplication app,
            IConsole console)
        {
            var currentVersion = _boostApplicationContext.Version;
            Console.WriteLine($"Installed:\t\t{currentVersion}");

            NugetPackageInfo? version = await _nugetService
                .GetNugetPackageInfoAsync("Boost.Tool", CommandAborded);

            if (version is { })
            {
                if (version.LatestStable is { })
                {
                    Console.WriteLine(
                        $"Latest:\t{version.LatestStable.Version}" +
                        $" ({version.LatestStable.Published:d})");
                }

                if (version.LatestPreRelease is { })
                {
                    Console.WriteLine(
                        $"Latest Pre-Release:\t{version.LatestPreRelease.Version}" +
                        $" ({version.LatestPreRelease.Published:d})");
                }

                console.WriteLine();

                var installed = SemVersion.Parse(currentVersion);


                if (version.LatestStable is { })
                {
                    if (SemVersion.Parse(version.LatestStable.Version) > installed)
                    {
                        console.Write(
                            "Run `dotnet tool update -g Boost.Tool` " +
                            "to install latest stable version");
                    }
                    else if (SemVersion.Parse(version.LatestStable.Version) == installed)
                    {
                        console.WriteLine(
                            "You have to latest stable version installed!",
                            ConsoleColor.Green);
                    }
                }

                if (version.LatestPreRelease is { })
                {
                    if (SemVersion.Parse(version.LatestPreRelease.Version) > installed)
                    {
                        console.Write(
                            $"Run `dotnet tool update -g Boost.Tool --version {version.LatestPreRelease.Version}` " +
                            "to install latest pre release version");
                    }
                    else if (SemVersion.Parse(version.LatestPreRelease.Version) == installed)
                    {
                        console.WriteLine(
                            "You have to latest and greatest pre-release installed!",
                            ConsoleColor.Green);
                    }
                }
            }
        }
    }
}
