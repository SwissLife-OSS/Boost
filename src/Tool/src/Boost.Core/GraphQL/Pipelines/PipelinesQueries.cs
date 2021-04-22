using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.GraphQL;
using Boost.Pipelines;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.Core.GraphQL
{
    [ExtendObjectType(RootTypes.Query)]
    public class PipelinesQueries
    {
        public Task<IEnumerable<Pipeline>> GetPipelinesAsync(
            GetPipelinesInput input,
            [Service] IPipelinesService pipelinesService,
            CancellationToken cancellationToken)
        {
            return pipelinesService.GetPipelinesAsync(
                input.ServiceId,
                input.RepositoryId,
                cancellationToken);
        }
    }
}
