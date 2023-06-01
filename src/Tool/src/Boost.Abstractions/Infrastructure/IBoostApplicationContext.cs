using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.Infrastructure;

public interface IBoostApplicationContext
{
    public DirectoryInfo WorkingDirectory { get; }

    public string? Version { get; }
}
