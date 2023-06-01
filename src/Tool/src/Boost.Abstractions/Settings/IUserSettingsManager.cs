using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Infrastructure;

namespace Boost.Settings
{
    public interface IUserSettingsManager
    {
        Task<UserSettings> GetAsync(CancellationToken cancellationToken);
        Task<WorkRoot?> GetWorkRootAsync(string? name, CancellationToken cancellationToken);
        Task SaveTokenGeneratorSettingsAsync(TokenGeneratorSettings tokenGeneratorSettings, CancellationToken cancellationToken);
        Task SaveWorkRootsAsync(IEnumerable<WorkRoot> workRoots, CancellationToken cancellationToken);

        Task SaveAsync(
            UserSettings settings,
            CancellationToken cancellationToken);
    }
}
