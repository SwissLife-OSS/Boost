using System;
using System.Reflection;
using Boost.AzureDevOps;
using Boost.Commands;
using Boost.Core.GraphQL;
using Boost.GitHub;
using Boost.GitHub.Commands;
using Boost.Infrastructure;
using Boost.Snapshooter.Commands;
using Boost.Web;
using Boost.WebApp;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.Tool;

[Command(
    Name = "boost",
    FullName = "A .NET global tool to boost your development",
    UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.StopParsingAndCollect,
    AllowArgumentSeparator = true)]
[VersionOptionFromMember(MemberName = nameof(GetVersion))]
[HelpOption]
[Subcommand(
    typeof(OpenUICommand),
    typeof(SnapshooterCommand),
    typeof(GitHubAuthCommand),
    typeof(CloneRepositoryCommand),
    typeof(OpenSolutionCommand),
    typeof(QuickActionsCommand),
    typeof(VersionCommand),
    typeof(SwitchRepositoryCommand),
    typeof(LocalProxyCommand),
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
                .AddSingleton(new AppSettings())
                .AddSingleton<IWebShellFactory, ConsoleWebShellFactory>()
                .AddSingleton<IWebServer>( c =>
                {
                    return new BoostWebServer(
                        c.GetRequiredService<IConsole>(),
                        new BoostCommandContext(c, (services) =>
                        {
                            services.AddGitHub();
                            services.AddAzureDevOps();
                            services.AddSnapshooter();

                            services.AddGraphQLServices((gql) =>
                            {
                                gql.AddSnapshooterTypes();
                                gql.AddGitHubTypes();
                                gql.AddAzureDevOpsTypes();

                            });
                        }, typeof(Program).Assembly));
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
        var startUI = Prompt.GetYesNo("Start UI?", true);

        if (startUI)
        {
            app.Execute("ui");
        }
    }

    public static string? GetVersion() => typeof(Program)
        .Assembly?
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
        .InformationalVersion;
}
