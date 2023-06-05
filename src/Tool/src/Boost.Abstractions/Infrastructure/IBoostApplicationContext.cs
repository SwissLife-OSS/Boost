using System.IO;

namespace Boost.Infrastructure;

public interface IBoostApplicationContext
{
    public DirectoryInfo WorkingDirectory { get; }

    public string? Version { get; }
}
