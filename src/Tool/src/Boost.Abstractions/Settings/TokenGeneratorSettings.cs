using System.Collections.Generic;

namespace Boost.Settings
{
    public class TokenGeneratorSettings
    {
        public IList<string> IdentityServers { get; set; } = new List<string>();

        public IList<CustomGrantSettings> CustomGrants { get; set; }
            = new List<CustomGrantSettings>();
    }
}
