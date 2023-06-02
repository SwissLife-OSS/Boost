namespace Boost.GraphQL;

public record ExecuteCommandInput(string Command)
{
    public string? WorkDirectory { get; init; }
}
