namespace Boost.GraphQL
{
    public record ExecuteCommandInput(string Command)
    {
        public string? Arguments { get; init; }
        public string? WorkDirectory { get; init; }
        public string? Shell { get; init; }
    }
}
