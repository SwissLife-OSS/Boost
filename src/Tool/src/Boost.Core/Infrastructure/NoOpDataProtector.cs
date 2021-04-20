using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boost.Infrastructure
{
    public class NoOpDataProtector : IUserDataProtector
    {
        public byte[] Protect(byte[] data)
        {
            return data;
        }

        public string Protect(string value)
        {
            return new string(value.Reverse().ToArray());
        }

        public string UnProtect(string value)
        {
            return new string(value.Reverse().ToArray());
        }

        public byte[] UnProtect(byte[] data)
        {
            return data;
        }
    }
}
