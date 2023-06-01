using System.Collections.Generic;
using Boost;

namespace Boost.Settings
{
    public class UserSettings
    {
        public string DefaultShell { get; set; } = default!;

        public IList<WorkRoot> WorkRoots { get; set; } = new List<WorkRoot>();

        public IList<ToolInfo> Tools { get; set; } = new List<ToolInfo>();

        public TokenGeneratorSettings TokenGenerator { get; set; }
            = new TokenGeneratorSettings();
    }
}
