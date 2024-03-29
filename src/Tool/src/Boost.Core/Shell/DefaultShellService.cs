using System.Runtime.InteropServices;
using Boost.Settings;

namespace Boost;

public class DefaultShellService : IDefaultShellService
{
    private readonly IUserSettingsManager _userSettingService;

    public DefaultShellService(IUserSettingsManager userSettingService)
    {
        _userSettingService = userSettingService;
    }

    public string GetDefault()
    {
        UserSettings settings = _userSettingService.GetAsync(default)
            .GetAwaiter()
            .GetResult();

        return settings?.DefaultShell ?? GetOSDefault();
    }

    internal static string GetOSDefault()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return "bash";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "zsh";
        }

        return "pwsh";
    }
}

