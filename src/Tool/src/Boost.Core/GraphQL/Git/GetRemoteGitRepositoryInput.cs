using System;

namespace Boost.Core.GraphQL
{
    public record GetRemoteGitRepositoryInput(Guid ServiceId, string Id);
}
