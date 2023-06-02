using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Boost.Infrastructure;

namespace Boost.Navigation;

public class AppNavigationService : IAppNavigationService
{
    private readonly IBoostCommandContext _commandContext;

    public AppNavigationService(IBoostCommandContext commandContext)
    {
        _commandContext = commandContext;
    }

    public AppNavigation GetNavigation()
    {
        string? name = _commandContext.ToolAssembly.GetManifestResourceNames()
            .FirstOrDefault(x => x.EndsWith("AppNavigation.json"));

        Stream? stream = _commandContext.ToolAssembly
            .GetManifestResourceStream(name!);

        if (stream is { })
        {
            using StreamReader reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            AppNavigation? appNav = JsonSerializer.Deserialize<AppNavigation>(json)!;

            foreach (MainNavigationItem? item in appNav.Items)
            {
                if (item.Id is null)
                {
                    item.Id = item.Route;
                }
                foreach (NavigationItem child in item.Children)
                {
                    if (child.Id is null)
                    {
                        child.Id = $"{item.Id}.{child.Route}";
                    }
                }
            }

            return appNav;
        }

        throw new ApplicationException("Could not load app navigation");
    }
}
