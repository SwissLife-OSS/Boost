using System;
using System.Collections.Generic;

namespace Boost.Infrastructure
{
    public class EncryptionKeySetting
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
            = new Dictionary<string, string>();
    }
}
