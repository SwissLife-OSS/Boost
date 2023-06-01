using System;
using System.IO;
using System.Linq;
using Boost.Infrastructure;
using Boost.Workspace;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.Commands;

[Command(
    Name = "os",
    FullName = "Open Solution",
    Description = "Opens Visual Studio solution"), HelpOption]
public class OpenSolutionCommand
{
    private readonly IBoostApplicationContext _applicationContext;

    public OpenSolutionCommand(IBoostApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }    

    public void OnExecute(IConsole console)
    {
        FileInfo[] solutions = _applicationContext.WorkingDirectory
            .GetFilesByExtensions(".slnf", ".sln")
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

            var index = console.ChooseFromList(
                solutions.Select(x => $"{x.Name} in {x.Directory!.Name}"));

            OpenSolution(solutions[index].FullName);
        }
        else
        {
            console.WriteLine(
                "No solution found in current directory.",
                ConsoleColor.Yellow);
        }
    }

    private void OpenSolution(string filename)
    {
        ProcessHelpers.Open(filename);
    }
}
