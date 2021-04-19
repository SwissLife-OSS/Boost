using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boost.Infrastructure
{
    public class NoOpDataProtector : IUserDataProtector
    {
        public byte[] Encrypt(byte[] data)
        {
            return data;
        }

        public byte[] Descypt(byte[] data)
        {
            return data;
        }
    }
}
