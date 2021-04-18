using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Pipelines
{
    public interface IPipelinesService
    {
        Task<IEnumerable<Pipeline>> GetPipelinesAsync(
            Guid serviceId,
            string repositoryId,
            CancellationToken cancellationToken);

        Task<IEnumerable<PipelineRun>> GetRunsAsync(
            Pipeline pipeline,
            int top,
            CancellationToken cancellationToken);
    }
}
