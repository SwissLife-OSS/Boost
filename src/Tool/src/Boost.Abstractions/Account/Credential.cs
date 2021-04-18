using System;

namespace Boost.Account
{
    public class Credential
    {
        public Credential()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public string Name { get; set; }

        public string Secret { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}
