using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Shell;

public interface IToolManager
{
    Task<IList<ToolInfo>> GetToolsAsync(CancellationToken cancellationToken);
    Task<string> GetToolPathAsync(string name, CancellationToken cancellationToken);
}
