using Boost.Git;
using Boost.Security;
using LiteDB;

namespace Boost.Data
{
    public interface IBoostDbContext
    {
        ILiteCollection<GitRepositoryIndex> GitRepos { get; }

        ILiteCollection<IdentityRequest> IdentityRequest { get; }
    }
}
