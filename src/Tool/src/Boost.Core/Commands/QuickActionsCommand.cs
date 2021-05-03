using System.Threading.Tasks;
using Boost.Infrastructure;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.Commands
{
    [Command(
        Name = "qa",
        FullName = "Quick actions",
        Description = "Execute quick action in working directory"), HelpOption]
    public class QuickActionsCommand : CommandBase
    {
        private readonly IBoostApplicationContext _applicationContext;

        public QuickActionsCommand(IBoostApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task OnExecute(CommandLineApplication app, IConsole console)
        {
            var utils = new WorkrootCommandUtils(app, console);

            await utils.ShowQuickActions(_applicationContext.WorkingDirectory.FullName);

        }
    }
}
