using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Account
{
    public interface ICredentialStore
    {
        string UserCredentialDirectory { get; }

        Task<IEnumerable<Credential>> GetAllAsync(CancellationToken cancellationToken);
        Task<Credential> GetAsync(string name, bool global, CancellationToken cancellationToken);
        Task<Credential> StoreAsync(string name, string secret, bool global, CancellationToken cancellationToken);
    }
}
