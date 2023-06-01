using System.Threading;
using System.Threading.Tasks;

namespace Boost.Workspace;

public interface IFileContentTypeHandler
{
    int Order { get; }

    bool CanHandle(WorkspaceFile file, HandleFileOptions options);

    Task HandleAsync(
        WorkspaceFile file,
        HandleFileOptions options,
        CancellationToken cancellationToken);
}

public record HandleFileOptions(string? Converter);
