using System;

namespace Boost.Core.GraphQL;

public record GetPipelinesInput(Guid ServiceId, string RepositoryId);
