using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.Git;

public interface IGitRemoteReference
{
    string Url { get; }
}
