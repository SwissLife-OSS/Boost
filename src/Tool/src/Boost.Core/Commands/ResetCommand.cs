using System.IO;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.Commands
{
    [Command(
        Name = "reset",
        FullName = "Reset Boost",
        Description = "Resets the boost application"), HelpOption]
    public class ResetCommand
    {
        public void OnExecute(IConsole console)
        {
            console.WriteLine("Reset Boost settings...");
        }
    }
}
