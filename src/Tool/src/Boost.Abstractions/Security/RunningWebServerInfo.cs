using System;

namespace Boost.Security;

public record RunningWebServerInfo(Guid Id, string Url)
{
    public DateTime StartedAt { get; } = DateTime.UtcNow;

    public string? Title { get; init; }
}
