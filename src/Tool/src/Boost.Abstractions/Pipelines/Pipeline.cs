using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.Pipelines
{
    public class Pipeline
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }
        public Guid ServiceId { get; set; }
        public string? WebUrl { get; set; }

        public IList<PipelineProperty> Properties { get; set; } = new List<PipelineProperty>();
    }

    public record PipelineProperty(string Name, string Value);

    public class PipelineRun
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Status { get; set; }

        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public string RequestedFor { get; set; }
        public string Reason { get; set; }
        public string? WebLink { get; set; }
        public Guid ServiceId { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
    }
}
