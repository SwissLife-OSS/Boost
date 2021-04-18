using System.Threading;
using System.Threading.Tasks;

namespace Boost.Core.Settings
{
    public interface ISettingsStore
    {
        Task<T?> GetAsync<T>(string fileName, string directory = "", CancellationToken cancellationToken = default);
        Task SaveAsync<T>(T userSettings, string fileName, string directory = "", CancellationToken cancellationToken = default);
    }
}