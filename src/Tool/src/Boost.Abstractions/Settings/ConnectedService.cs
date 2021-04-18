using System;
using System.Collections.Generic;

namespace Boost.Settings
{
    public class ConnectedService
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string Type { get; set; } = default!;

        public string? DefaultWorkRoot { get; set; } = default!;

        public IList<ConnectedServiceProperty> Properties { get; set; }
            = new List<ConnectedServiceProperty>();
    }

    public record ConnectedServiceProperty(string Name, string Value);

    public record ConnectedServiceType(
        string Name,
        IEnumerable<ConnectedServiceFeature> Features);

    public enum ConnectedServiceFeature
    {
        GitRemoteRepository,
        Pipelines
    }

    public interface IConnectedService
    {
        Guid Id { get; set; }
        string Name { get; }
        string Type { get; }
        string? DefaultWorkRoot { get; }
    }
}
