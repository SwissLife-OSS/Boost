using System.Collections.Generic;
using Boost.Infrastructure;

namespace Boost.Settings
{
    public class UserSettings
    {
        public string DefaultShell { get; set; } = default!;

        public IList<WorkRoot> WorkRoots { get; set; } = new List<WorkRoot>();

        public TokenGeneratorSettings TokenGenerator { get; set; }
            = new TokenGeneratorSettings();

        public EncryptionKeySetting Encryption { get; set; } = new EncryptionKeySetting();
    }
}
