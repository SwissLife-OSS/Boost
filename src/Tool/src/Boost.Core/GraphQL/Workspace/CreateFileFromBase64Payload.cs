using Boost.Workspace;

namespace Boost.GraphQL;

public class CreateFileFromBase64Payload
{
    public CreateFileFromBase64Payload(WorkspaceFile file)
    {
        File = file;
    }

    public WorkspaceFile File { get; }
}

