namespace Boost.GraphQL
{
    public record OpenInVSCodeInput()
    {
        public string? File { get; init; }

        public string? Directory { get; init; }
    }
}

