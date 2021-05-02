using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.Git;
using Boost.Settings;
using Boost.Workspace;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.Commands
{
    [Command(
        Name = "index",
        FullName = "Index Repositories",
        Description = "Index Repositories"), HelpOption]
    public class IndexRepositoriesCommand : CommandBase
    {
        private readonly IUserSettingsManager _settingsManager;
        private readonly ILocalRepositoryIndexer _localRepositoryIndexer;
        private readonly IWorkspaceService _workspaceService;

        public IndexRepositoriesCommand(
            IUserSettingsManager settingsManager,
            ILocalRepositoryIndexer localRepositoryIndexer,
            IWorkspaceService workspaceService)
        {
            _settingsManager = settingsManager;
            _localRepositoryIndexer = localRepositoryIndexer;
            _workspaceService = workspaceService;
        }

        public async Task OnExecute(CommandLineApplication app, IConsole console)
        {
            var utils = new WorkrootCommandUtils(app, console);

            IEnumerable<WorkRoot> workroots = await utils.GetWorkRootsAsync(CommandAborded);

            foreach (WorkRoot wr in workroots)
            {
                await _localRepositoryIndexer.IndexWorkRootAsync(
                    wr,
                    onProgress: (msg) => console.WriteLine(msg),
                    CommandAborded);
            }

            console.WriteLine("Repo indexing completed", ConsoleColor.Green);
        }
    }
}
