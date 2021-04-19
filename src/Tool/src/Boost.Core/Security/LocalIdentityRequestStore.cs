using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        public IEnumerable<IdentityRequest> SearchRequest(SearchIdentityRequest searchRequest)
        {
            IEnumerable<IdentityRequest> requests = _dbContext.IdentityRequest
                .Find(x => x.Type == searchRequest.Type)
                .OrderByDescending(x => x.CreatedAt);

            return requests;
        }

        public IdentityRequest Save<T>(SaveIdentityRequest<T> request)
            where T : class
        {
            var jsonData = JsonSerializer.SerializeToUtf8Bytes(request.Data);

            IdentityRequest toSave = new IdentityRequest();

            if (request.Id.HasValue)
            {
                IdentityRequest? existing = _dbContext.IdentityRequest.FindById(request.Id.Value);

                if (existing is { })
                {
                    toSave = existing;
                }
            }
            else
            {
                toSave.Id = Guid.NewGuid();
            }

            toSave.Name = request.Name;
            toSave.Tags = request.Tags.ToList();
            toSave.Type = request.Type;
            toSave.Data = _userDataProtector.Encrypt(jsonData);

            _dbContext.IdentityRequest.Upsert(toSave);

            return toSave;
        }
    }
}
