using System;
using System.Collections.Generic;

namespace Boost.Infrastructure
{
    public class DataProtectorKeyRing
    {
        public Guid? ActiveKeyId { get; set; }

        public IList<EncryptionKeySetting> Protectors { get; set; } = new List<EncryptionKeySetting>();
    }
}
