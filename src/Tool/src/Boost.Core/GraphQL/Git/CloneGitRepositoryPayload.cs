namespace Boost.Core.GraphQL;

public class CloneGitRepositoryPayload
{
    public CloneGitRepositoryPayload(int errorCode, string directory)
    {
        ErrorCode = errorCode;
        Directory = directory;
    }

    public int ErrorCode { get; }
    public string Directory { get; }
}
