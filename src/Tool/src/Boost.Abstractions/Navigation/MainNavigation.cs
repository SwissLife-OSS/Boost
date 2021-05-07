using System.Collections.Generic;

namespace Boost.Navigation
{
    public class AppNavigation
    {
        public string? Title { get; set; } = default!;

        public IEnumerable<MainNavigationItem> Items { get; set; }
            = new List<MainNavigationItem>();
    }

    public class NavigationItem
    {
        public string? Id { get; set; } = default!;

        public string Title { get; set; } = default!;

        public string Route { get; set; } = default!;

        public string? Icon { get; set; } = default!;
    }

    public class MainNavigationItem : NavigationItem
    {
        public IEnumerable<NavigationItem> Children { get; set; }
            = new List<NavigationItem>();
    }
}
