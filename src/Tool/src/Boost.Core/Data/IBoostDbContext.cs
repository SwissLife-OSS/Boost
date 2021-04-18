using Boost.Git;
using LiteDB;

namespace Boost.Data
{
    public interface IBoostDbContext
    {
        ILiteCollection<GitRepositoryIndex> GitRepos { get; }
    }
}