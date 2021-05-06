using System;
using System.Threading.Tasks;
using Boost.Infrastructure;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.Commands
{
    [Command(
        Name = "version",
        FullName = "Version",
        Description = "Get current Version and check for new version"), HelpOption]
    public class VersionCommand : CommandBase
    {
        private readonly IVersionChecker _versionChecker;

        public VersionCommand(IVersionChecker versionChecker
)
        {
            _versionChecker = versionChecker;
        }

        public async Task OnExecute(
            CommandLineApplication app,
            IConsole console)
        {
            BoostVersionInfo version = await _versionChecker.GetVersionInfo(CommandAborded);

            if (version is { })
            {
                Console.WriteLine("Boost Version info");
                Console.WriteLine("------------------");
                Console.WriteLine($"Installed:\t{version.Installed}");

                if (version.Latest is { })
                {
                    Console.WriteLine(
                        $"Latest:\t\t{version.Latest.Version}" +
                        $" ({version.Latest.Published:d})");
                }

                if (version.PreRelease is { })
                {
                    Console.WriteLine(
                        $"Latest Pre-Release:\t{version.PreRelease.Version}" +
                        $" ({version.PreRelease.Published:d})");
                }

                console.WriteLine();

                if (version.NewerAvailable)
                {
                    console.Write(
                        $"Run `dotnet tool update -g {version.PackageId}` " +
                        "to install latest stable version");
                }


                if (version.NewerPreReleaseAvailable)
                {
                    console.Write(
                        $"Run `dotnet tool update -g {version.PackageId} --version {version.PreRelease!.Version}` " +
                        "to install latest pre release version");
                }
            }
        }
    }
}
