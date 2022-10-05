using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Pipelines;
using Microsoft.TeamFoundation.Build.WebApi;

namespace Boost.AzureDevOps
{
    public interface IAzureDevOpsPipelinesClient
    {
        Task<IEnumerable<Pipeline>> GetPipelinesAsync(
            Guid serviceId,
            string repositoryId,
            CancellationToken cancellationToken);

        Task<IEnumerable<Build>> GetRunsAsync(
            Guid serviceId,
            Guid projectId,
            int id,
            int top = 5,
            CancellationToken cancellationToken = default);
    }
}
