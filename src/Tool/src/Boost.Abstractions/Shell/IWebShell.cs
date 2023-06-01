using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boost;

public interface IWebShell
{
    Task<int> ExecuteShellAsync(string arguments, string? workingDirectory);
    Task<int> ExecuteGitAsync(IEnumerable<string> arguments, string directory);
    Task<int> ExecuteAsync(string targetFilename, string arguments, string? workingDirectory);
}
