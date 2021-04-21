using System;
using System.Reflection;
using Boost.Commands;
using Boost.GitHub.Commands;
using Boost.Snapshooter.Commands;
using Boost.Web;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.Tool
{
    [Command(
        Name = "boost",
        FullName = "A .NET global tool to boost your development")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    [HelpOption]
    [Subcommand(
        typeof(OpenUICommand),
        typeof(SnapshooterCommand),
        typeof(GitHubAuthCommand),
        typeof(CloneRepositoryCommand),
        typeof(IndexRepositoriesCommand))]
    class Program
    {
        static int Main(string[] args)
        {
            LogConfiguration.CreateLogger();

            using (ServiceProvider services = new ServiceCollection()
                    .AddSingleton(PhysicalConsole.Singleton)
                    .AddSingleton<IReporter>(provider =>
                        new ConsoleReporter(provider.GetRequiredService<IConsole>()))
                    .AddToolServices()
                    .AddSingleton<IWebServer>( c =>
                    {
                        return new BoostWebServer(
                            c.GetRequiredService<IConsole>(),
                            new BoostCommandContext(c));
                    })
                    .BuildServiceProvider())
            {
                var app = new CommandLineApplication<Program>();
                app.Conventions
                    .UseDefaultConventions()
                    .UseConstructorInjection(services);

                return app.Execute(args);
            }
        }

        public void OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
        }

        public static string? GetVersion() => typeof(Program)
            .Assembly?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion;
    }

    public record BoostCommandContext(IServiceProvider Services);
}
