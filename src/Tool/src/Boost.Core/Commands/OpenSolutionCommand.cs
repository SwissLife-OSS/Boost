using System;
using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.Commands
{
    [Command(
        Name = "os",
        FullName = "Open Solution",
        Description = "Opens Visual Studio solution"), HelpOption]
    public class OpenSolutionCommand
    {
        public void OnExecute(IConsole console)
        {
            FileInfo[] solutions = new DirectoryInfo("")
                .EnumerateFiles("*.sln", SearchOption.AllDirectories)
                .ToArray();

            if (solutions.Count() == 1)
            {
                OpenSolution(solutions[0].FullName);
            }
            else if (solutions.Count() > 1)
            {
                console.WriteLine("Following solutions found:");
                console.WriteLine("---------------------------");
                console.WriteLine();

                var index = console.ChooseFromList(solutions.Select(x => $"{x.Name} in {x.Directory!.Name}"));
                OpenSolution(solutions[index].FullName);
            }
            else
            {
                console.WriteLine("No solution found.", ConsoleColor.Yellow);
            }
        }

        private void OpenSolution(string filename)
        {
            ProcessHelpers.Open(filename);
        }
    }
}
