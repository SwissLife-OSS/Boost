using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boost.Git
{

    public record GitLocalCommit(string Sha, string Message, DateTimeOffset CreatedAt)
    {
        public string? Author { get; init; }
    }

    public class GitRepositoryIndex
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string WorkingDirectory { get; set; }

        [NotMapped]
        public IGitRemoteReference? RemoteReference { get; set; }

        public Guid? ServiceId { get; set; }

        public string WorkRoot { get; set; }

        public DateTimeOffset? LastCommitDate { get; set; }
    }

    public class GitLocalRepository
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string WorkingDirectory { get; set; }

        public Guid? RemoteServiceId { get; set; }

        public string WorkRoot { get; set; }

        public IEnumerable<GitTag> Tags { get; set; } = new List<GitTag>();
        public IEnumerable<GitBranch> Branches { get; set; } = new List<GitBranch>();

        public GitHead Head { get; set; }

        public IEnumerable<GitLocalCommit> Commits { get; set; } = Array.Empty<GitLocalCommit>();
        public IGitRemoteReference? RemoteReference { get; set; }
    }

    public record GitBranch(string Name);

    public record GitTag(string Name);

    public record GitHead(string Name)
    {
        public int? AheadBy { get; init; }
        public int? BehindBy { get; init; }
        public string? Message { get; init; }
        public string? Sha { get; init; }
    }

    public class GitRemoteRepository
    {
        public string Id { get; set; }

        public string Source { get; set; } = default!;

        public string FullName { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string WebUrl { get; set; } = default!;

        public string Url { get; set; } = default!;
        public Guid ServiceId { get; set; }
    }
}
