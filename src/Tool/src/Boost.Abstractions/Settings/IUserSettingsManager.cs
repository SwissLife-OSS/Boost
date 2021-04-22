using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Settings
{
    public interface IUserSettingsManager
    {
        Task<UserSettings> GetAsync(CancellationToken cancellationToken);
        Task<string?> GetWorkRootAsync(string? name, CancellationToken cancellationToken);
        Task SaveTokenGeneratorSettingsAsync(TokenGeneratorSettings tokenGeneratorSettings, CancellationToken cancellationToken);
        Task SaveWorkRootsAsync(IEnumerable<WorkRoot> workRoots, CancellationToken cancellationToken);
    }
}
