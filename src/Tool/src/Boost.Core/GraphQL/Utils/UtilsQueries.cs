using System;
using System.Collections.Generic;
using Boost.GraphQL;
using Boost.Utils;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.Core.GraphQL;

[ExtendObjectType(RootTypes.Query)]
public class UtilsQueries
{
    public string Encode(
        [Service] IEncodingService encodingService,
        string value,
        EncodingType type)
    {
        return encodingService.Encode(value, type);
    }

    public string Decode(
        [Service] IEncodingService encodingService,
        string value,
        EncodingType type)
    {
        return encodingService.Decode(value, type);
    }

    public string? CreateHash(
        [Service] ISecurityUtils securityUtils,
        CreateHashInput input)
    {
        return securityUtils.CreateHash(input.Value, input.Alg);
    }

    public IEnumerable<string> CreateGuids(int count = 1, string? format = null)
    {
        for (int i = 0; i < count; i++)
        {
            yield return Guid.NewGuid().ToString(format);
        }
    }

    public IEnumerable<string> ParseGuid(string value)
    {
        if (Guid.TryParse(value, out Guid parsed))
        {
            yield return parsed.ToString();
            yield return parsed.ToString("N");
            yield return $"NUUID(\"{parsed}\")";
        }
        else
        {
            yield return "Invalid Guid";
        }
    }
}
