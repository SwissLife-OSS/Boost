using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.Settings;
using Boost.Web;
using Boost.Workspace;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Boost.Commands
{
    public class WorkrootCommandUtils
    {
        private readonly CommandLineApplication _app;
        private readonly IConsole _console;

        public WorkrootCommandUtils(CommandLineApplication app, IConsole console)
        {
            _app = app;
            _console = console;
        }

        public async Task<IEnumerable<WorkRoot>> GetWorkRootsAsync(
            CancellationToken cancellationToken)
        {
            IUserSettingsManager settingsManager = _app.GetRequiredService<IUserSettingsManager>();

            UserSettings? settings = await settingsManager.GetAsync(cancellationToken);

            if (settings.WorkRoots.Count() == 0)
            {
                _console.WriteLine("No work roots configured!");
                _console.WriteLine("You can configure you work roots in the UI or directly in:");
                _console.WriteLine($"{SettingsStore.UserSettingsFilePath}.");

                var options = new List<string>
                {
                    "Open UI",
                    "Open User settings in code",
                    $"Add {Directory.GetCurrentDirectory()} as work root",
                    "Cancel"
                };

                var index = _console.ChooseFromList(options);

                switch (index)
                {
                    case 0:
                        IWebServer? webServer = _app.GetRequiredService<IWebServer>();
                        await webServer.StartAsync(3003, "settings");
                        break;
                    case 1:
                        //first ensure file exists
                        if (!File.Exists(SettingsStore.UserSettingsFilePath))
                        {
                            await AddCurrentWorkroot(settingsManager, cancellationToken);
                        }
                        IWorkspaceService workspaceService = _app.GetRequiredService<IWorkspaceService>();
                        await workspaceService.OpenFileInCode(SettingsStore.UserSettingsFilePath);
                        break;
                    case 2:
                        await AddCurrentWorkroot(settingsManager, cancellationToken);
                        break;
                    default:
                        break;
                }
            }

            return settings.WorkRoots;
        }

        private async Task AddCurrentWorkroot(
            IUserSettingsManager settingsManager,
            CancellationToken cancellationToken)
        {
            var workroots = new List<WorkRoot>(){

                new WorkRoot
                {
                    Name = "AutoAdded",
                    Path = Directory.GetCurrentDirectory(),
                    IsDefault = true,
                }
            };

            await settingsManager.SaveWorkRootsAsync(workroots, cancellationToken);
        }
    }
}
