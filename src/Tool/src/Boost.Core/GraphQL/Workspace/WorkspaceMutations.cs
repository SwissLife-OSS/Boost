using System.Threading;
using System.Threading.Tasks;
using Boost.Workspace;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.GraphQL;

[ExtendObjectType(RootTypes.Mutation)]
public class WorkspaceMutations
{
    public Task<int> OpenInVSCode(
        [Service] IWorkspaceService workspaceService,
        OpenInVSCodeInput input)
    {
        if (input.File is { })
        {
            return workspaceService.OpenFileInCode(input.File);
        }
        else
        {
            return workspaceService.OpenInCode(input.Directory);
        }
    }

    public Task<int> OpenFile(
        [Service] IWorkspaceService workspaceService,
        string fileName)
    {
        return workspaceService.OpenFile(fileName);
    }

    public Task<int> OpenInExplorer(
        [Service] IWorkspaceService workspaceService,
        string directory)
    {
        return workspaceService.OpenInExplorer(directory);
    }

    public Task<int> OpenInTerminal(
        [Service] IWorkspaceService workspaceService,
        string directory)
    {
        return workspaceService.OpenInTerminal(directory);
    }

    public Task<int> RunSuperBoostAsync(
        [Service] IWorkspaceService workspaceService,
        RunSuperBoostInput input)
    {
        return workspaceService.RunSuperBoostAsync(input.Name, input.Directory);
    }

    public Task<int> ExecuteFileAction(
        [Service] IWorkspaceService workspaceService,
        ExecuteFileActionInput input)
    {
        return workspaceService.ExecuteFileActionAsync(input.File, input.Action);
    }

    public async Task<CreateFileFromBase64Payload> CreateFileFromBase64Async(
        [Service] IWorkspaceService workspaceService,
        CreateFileFromBase64Input input,
        CancellationToken cancellationToken
        )
    {
        WorkspaceFile file = await workspaceService.CreateFileFromBase64Async(
            input.Value,
            input.FileType,
            cancellationToken);

        return new CreateFileFromBase64Payload(file);
    }
}

