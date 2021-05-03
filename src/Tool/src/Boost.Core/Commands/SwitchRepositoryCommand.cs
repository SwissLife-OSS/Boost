using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boost.Git;
using Boost.Settings;
using Boost.Workspace;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.Commands
{
    [Command(
        Name = "sr",
        FullName = "Switch repository",
        Description = "Switches Git repository"), HelpOption]
    public class SwitchRepositoryCommand : CommandBase
    {
        private readonly IGitLocalRepositoryService _localRepositoryService;
        private readonly IWorkspaceService _workspaceService;

        public SwitchRepositoryCommand(
            IGitLocalRepositoryService localRepositoryService,
            IWorkspaceService workspaceService)
        {
            _localRepositoryService = localRepositoryService;
            _workspaceService = workspaceService;
        }

        [Argument(0, "SearchText", ShowInHelpText = true)]
        public string SearchText { get; set; } = default!;

        public async Task OnExecute(CommandLineApplication app, IConsole console)
        {
            var utils = new WorkrootCommandUtils(app, console);
            IEnumerable<WorkRoot> workroots = await utils.GetWorkRootsAsync(CommandAborded);

            console.Write("Searching....");

            GitLocalRepository[] repos = _localRepositoryService
                .Search(SearchText)
                .ToArray();

            console.ClearLine();

            if (repos.Count() == 0)
            {
                console.WriteLine($"\nNo repo found with search term: {SearchText}.");
                console.WriteLine($"Make sure you have configured your workroots correctly " +
                                  $"and you did index your workroots." +
                                  $"\nRun `boo index` to start indexing all your work roots." +
                                  $"\n\nSearched workroots: " +
                                  $"\n-------------------");

                foreach (Settings.WorkRoot wr in workroots)
                {
                    console.WriteLine($"{wr.Path} | {wr.Name}");
                }

                return;
            }

            int index = 0;

            if (repos.Count() > 1)
            {
                index = console.ChooseFromList(
                    repos.Select(x => $"{x.Name} ({x.WorkRoot})"));
            }

            GitLocalRepository? repo = repos[index];

            await utils.ShowQuickActions(repo.WorkingDirectory);
        }
    }
}
