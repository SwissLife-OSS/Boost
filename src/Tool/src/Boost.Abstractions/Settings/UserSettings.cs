using System.Collections.Generic;

namespace Boost.Settings
{
    public class UserSettings
    {
        public string DefaultShell { get; set; } = default!;

        public IList<WorkRoot> WorkRoots { get; set; } = new List<WorkRoot>();

        public TokenGeneratorSettings TokenGenerator { get; set; }
            = new TokenGeneratorSettings();

        public DataEncryptionSettings Encryption { get; set; } = new DataEncryptionSettings();
    }

    public class DataEncryptionSettings
    {
        public EnryptionType Type { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
            = new Dictionary<string, string>();
    }

    public enum EnryptionType
    {
        None,
        X509Certificate
    }
}
