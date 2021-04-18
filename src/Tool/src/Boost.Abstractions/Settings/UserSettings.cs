using System.Collections.Generic;

namespace Boost.Settings
{
    public class UserSettings
    {
        public string DefaultShell { get; set; }

        public IList<WorkRoot> WorkRoots { get; set; } = new List<WorkRoot>();
    }
}
