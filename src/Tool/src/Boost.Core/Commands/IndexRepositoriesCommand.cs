using System.Linq;
using System.Threading.Tasks;
using Boost.Git;
using Boost.Settings;
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
        private readonly LocalRepositoryIndexer _localRepositoryIndexer;

        public IndexRepositoriesCommand(
            IUserSettingsManager settingsManager,
            LocalRepositoryIndexer localRepositoryIndexer)
        {
            _settingsManager = settingsManager;
            _localRepositoryIndexer = localRepositoryIndexer;
        }

        public async Task OnExecute(IConsole console)
        {
            UserSettings? settings = await _settingsManager.GetAsync(CommandAborded);

            foreach (WorkRoot wr in settings.WorkRoots)
            {
                await _localRepositoryIndexer.IndexWorkRootAsync(wr, CommandAborded);
            }
        }
    }
}
