using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boost.Git;
using Boost.Security;
using LiteDB;

namespace Boost.Data
{
    public class BoostDbContext : IBoostDbContext
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
    }
}
