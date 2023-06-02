using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Pipelines;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.Core.GraphQL;

public class PipelineType : ObjectType<Pipeline>
{
    protected override void Configure(IObjectTypeDescriptor<Pipeline> descriptor)
    {
        descriptor
            .Field("runs")
            .Argument("top", a =>
            {
                a.DefaultValue(5);
                a.Type(typeof(int));
            })
            .ResolveWith<Resolvers>(_ => _.GetRunsAsync(default!, default!, default!, default!));
    }

    class Resolvers
    {
        public  Task<IEnumerable<PipelineRun>> GetRunsAsync(
            [Parent] Pipeline pipeline,
            [Service] IPipelinesService pipelinesService,
            int top,
            CancellationToken cancellationToken)
        {
            return pipelinesService.GetRunsAsync(pipeline, top, cancellationToken);
        }
    }
}
