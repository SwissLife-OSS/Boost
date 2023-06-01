using System.Collections.Generic;

namespace Boost.GraphQL;

public record AuthorizeRequestInput(
    string Authority,
    string ClientId,
    IEnumerable<string> ResponseTypes,
    string? Secret,
    IEnumerable<string> Scopes,
    bool UsePkce,
    bool SaveTokens)
{
    public int? Port { get; init; }

    public string? RequestId { get; init; }
}
