using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Boost.Settings;

namespace Boost.Shell;

public class ToolManager : IToolManager
{
    private readonly IUserSettingsManager _userSettingsManager;

    public ToolManager(IUserSettingsManager userSettingsManager)
    {
        _userSettingsManager = userSettingsManager;
    }

    public async Task<IList<ToolInfo>> GetToolsAsync(CancellationToken cancellationToken)
    {
        UserSettings settings = await _userSettingsManager.GetAsync(cancellationToken);

        if (!settings.Tools.Any())
        {
            settings.Tools = await DiscoverToolsAsync(cancellationToken);

            await _userSettingsManager.SaveAsync(settings, cancellationToken);
        }

        return settings.Tools;
    }

    public async Task<string> GetToolPathAsync(string name, CancellationToken cancellationToken)
    {
        IList<ToolInfo> tools = await GetToolsAsync(cancellationToken);

        ToolInfo? tool = tools.FirstOrDefault(x => x.Name.Equals(
            name,
            StringComparison.InvariantCultureIgnoreCase));

        return tool?.Path ?? name;
    }

    private async Task<IList<ToolInfo>> DiscoverToolsAsync(CancellationToken cancellationToken)
    {
        var tools = new List<ToolInfo>();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            tools.Add(new ToolInfo {
                            Name = "bash",
                            Path = "/bin/bash",
                            Type = ToolType.Shell,
                            IsDefault = true });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            tools.Add(new ToolInfo {
                Name = "zsh",
                Path = "/bin/zsh",
                Type = ToolType.Shell,
                IsDefault = true });

            tools.Add(new ToolInfo {
                Name = "bash",
                Path = "/bin/zsh",
                Type = ToolType.Shell,
                IsDefault = true });
        }
        else
        {
            tools.Add(new ToolInfo {
                Name = "powershell",
                Path = "pwsh",
                Type = ToolType.Shell,
                IsDefault = true });
        }

        return tools;
    }
}
