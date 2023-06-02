using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Workspace;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.GraphQL;

[ExtendObjectType(Name = RootTypes.Query)]
public class WorkspaceQueries
{
    public WorkspaceContext GetWorkspace(
        [Service] IWorkspaceService workspaceService)
    {
        return workspaceService.GetWorkspace();
    }

    public IEnumerable<FileSystemItem> GetDirectoryChildren(
        string path,
        [Service] IWorkspaceService workspaceService)
    {
        return workspaceService.GetFileSystemItems(path);
    }

    public Task<WorkspaceFile> GetFileAsync(
        GetFileInput input,
        [Service] IWorkspaceService workspaceService,
        CancellationToken cancellationToken)
    {
        return workspaceService.GetFileAsync(new GetFileRequest(input.FileName)
        {
            Converter = input.Converter
        }, cancellationToken);
    }

    public IEnumerable<QuickAction> GetQuickActions(
        string? directory,
        [Service] IWorkspaceService workspaceService)
    {
        return workspaceService.GetQuickActions(directory);
    }
}

public record GetFileInput(string FileName)
{
    public string? Converter { get; init; }
}

