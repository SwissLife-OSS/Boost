namespace Boost
{
    public record ShellCommand(string Command)
    {
        public string Arguments { get; init; }
        public string WorkDirectory { get; init; }
    }
}
