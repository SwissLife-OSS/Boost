namespace Boost.Workspace
{
    public record GetFileRequest(string FileName)
    {
        public string? Converter { get; init; }
    }
}
