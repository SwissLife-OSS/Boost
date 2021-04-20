using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Data;
using Boost.Infrastructure;

namespace Boost.Security
{
    public class LocalIdentityRequestStore : IIdentityRequestStore
    {
        private readonly IBoostDbContext _dbContext;
        private readonly IUserDataProtector _userDataProtector;

        public LocalIdentityRequestStore(
            IBoostDbContext dbContext,
            IUserDataProtector userDataProtector)
        {
            _dbContext = dbContext;
            _userDataProtector = userDataProtector;
        }

        public Task<IdentityRequestItem> SaveAsync(
            SaveIdentityRequestInput request,
            CancellationToken cancellationToken)
        {
            IdentityRequestItem toSave = new IdentityRequestItem();

            if (request.Id.HasValue)
            {
                IdentityRequestItem? existing = _dbContext.IdentityRequest
                    .FindById(request.Id.Value);

                if (existing is { })
                {
                    toSave = existing;
                }
            }
            else
            {
                toSave.Id = Guid.NewGuid();
                toSave.CreatedAt = DateTime.UtcNow;
            }

            toSave.ModifiedAt = DateTime.UtcNow;
            toSave.Name = request.Name;
            toSave.Tags = request.Tags.ToList();
            toSave.Type = request.Type;
            toSave.Data = request.Data;

            if (toSave.Data.Secret is { })
            {
                toSave.Data.Secret = _userDataProtector.Protect(toSave.Data.Secret);
            }

            _dbContext.IdentityRequest.Upsert(toSave);

            return Task.FromResult(toSave);
        }

        public Task<IdentityRequestItem> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            IdentityRequestItem request = _dbContext.IdentityRequest
                 .FindById(id);

            if (request.Data.Secret is { })
            {
                request.Data.Secret = _userDataProtector.UnProtect(request.Data.Secret);
            }

            return Task.FromResult(request);
        }

        public Task<IEnumerable<IdentityRequestItem>> SearchAsync(SearchIdentityRequest searchRequest, CancellationToken cancellationToken)
        {
            IEnumerable<IdentityRequestItem> requests = _dbContext.IdentityRequest
                .Find(x => x.Type == searchRequest.Type)
                .OrderByDescending(x => x.CreatedAt);

            return Task.FromResult(requests);
        }
    }
}
