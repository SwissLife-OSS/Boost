using System;
using System.Collections.Generic;
using Boost.Settings;

namespace Boost.Core.GraphQL;

public record SaveConnectedServiceInput(
    Guid? id,
    string Name,
    string Type,
    IEnumerable<ConnectedServiceProperty> Properties)
{
    public string? DefaultWorkRoot { get; init; }
};
