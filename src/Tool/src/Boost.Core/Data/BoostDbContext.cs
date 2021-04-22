using System;
using Boost.Git;
using Boost.Security;
using LiteDB;

namespace Boost.Data
{
    public class BoostDbContext : IBoostDbContext, IDisposable
    {
        private readonly LiteDatabase _db;

        public BoostDbContext(LiteDatabase db)
        {
            _db = db;
        }

        public ILiteCollection<GitRepositoryIndex> GitRepos
            => _db.GetCollection<GitRepositoryIndex>("GitRepos");

        public ILiteCollection<IdentityRequestItem> IdentityRequest
             => _db.GetCollection<IdentityRequestItem>("IdentityRequests");

        public void Dispose()
        {
            try
            {
                //_db.Dispose();
            }
            catch
            {

            }
        }
    }
}
