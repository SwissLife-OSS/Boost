using System;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Boost.Commands;

namespace Boost
{
    [Command(Name = "boost",
          FullName = "Boost tools",
          Description = "Boost tools")]
    [HelpOption]
    [VersionOptionFromMember(MemberName = "GetVersion")]
    [Subcommand(
        typeof(OpenUICommand),
        typeof(OpenSolutionCommand),
        typeof(CloneRepositoryCommand),
        typeof(ResetCommand))]
    class Program
    {
        public static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        public void OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
        }

        private string GetVersion()
        {
            return typeof(Program)
                .Assembly?
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;
        }
    }
}
