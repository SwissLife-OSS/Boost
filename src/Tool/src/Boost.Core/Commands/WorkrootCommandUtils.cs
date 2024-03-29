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

namespace Boost.Commands;

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
                    var url = await webServer.StartAsync(3003);
                    ProcessHelpers.OpenBrowser(url + "/settings");
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

    public async Task ShowQuickActions(string path)
    {
        IWorkspaceService workspaceService = _app.GetRequiredService<IWorkspaceService>();
        var dir = new DirectoryInfo(path);

        QuickAction[] quickActions =
            workspaceService.GetQuickActions(path).ToArray();

        Console.WriteLine($"Quick actions: {dir.Name}");
        Console.WriteLine($"------------------------------------");

        var actionIndex = _console.ChooseFromList(
            quickActions.Select(x => x.ToString()));

        QuickAction? action = quickActions[actionIndex];

        switch (action.Type)
        {
            case QuickActionTypes.OpenVisualStudioSolution:
                ProcessHelpers.Open(action.Value);
                break;
            case QuickActionTypes.OpenDirectoryInExplorer:
                await workspaceService.OpenInExplorer(action.Value);
                break;
            case QuickActionTypes.OpenDirectoryInCode:
                await workspaceService.OpenInCode(action.Value);
                break;
            case QuickActionTypes.OpenDirectoryInTerminal:
                await workspaceService.OpenInTerminal(action.Value);
                break;
            case QuickActionTypes.RunSuperBoost:
                await workspaceService.RunSuperBoostAsync(action.Title, path);
                break;
        }
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
