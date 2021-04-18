using System;

namespace Boost.Git
{
    public record GitRemoteCommit(
        string Id,
        string Message,
        DateTime CreatedAt,
        string? Author)
    {
        public string? WebUrl { get; init; }
    }
}
