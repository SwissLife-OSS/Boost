using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Settings;

namespace Boost.Shell;

public class ToolManager
{
    private readonly IUserSettingsManager _userSettingsManager;

    public ToolManager(IUserSettingsManager userSettingsManager)
    {
        _userSettingsManager = userSettingsManager;
    }

    public async Task<IList<ToolInfo>> GetTools(CancellationToken cancellationToken)
    {
        UserSettings settings = await _userSettingsManager.GetAsync(cancellationToken);

        if (!settings.Tools.Any())
        {

        }

        return null;
    }

    public async Task<IList<ToolInfo>> DiscoverToolsAsync(CancellationToken cancellationToken)
    {
        return null;
    }
}

