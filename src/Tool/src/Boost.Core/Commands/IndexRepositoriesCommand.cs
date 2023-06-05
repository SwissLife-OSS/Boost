using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boost.Git;
using Boost.Settings;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.Commands;

[Command(
    Name = "index",
    FullName = "Index Repositories",
    Description = "Index Repositories"), HelpOption]
public class IndexRepositoriesCommand : CommandBase
{
    private readonly ILocalRepositoryIndexer _localRepositoryIndexer;

    public IndexRepositoriesCommand(
        ILocalRepositoryIndexer localRepositoryIndexer)
    {
        _localRepositoryIndexer = localRepositoryIndexer;
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
