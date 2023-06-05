using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boost;

public interface IWebShell
{
    Task<int> ExecuteShellAsync(string arguments, string? workingDirectory);
    Task<int> ExecuteGitAsync(IEnumerable<string> arguments, string directory);
    Task<int> ExecuteAsync(string targetFilename, string arguments, string? workingDirectory);
    Task<int> ExecuteGitManyAsync(IEnumerable<string[]> commands, string directory);
    Task<int> ExecuteManyAsync(string targetFilename, string[] commands, string? workingDirectory);
}
