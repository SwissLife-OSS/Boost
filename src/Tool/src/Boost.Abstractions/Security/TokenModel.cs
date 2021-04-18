using System;
using System.Collections.Generic;

namespace Boost.Security
{
    public record ClaimCategoryMap(string Type, ClaimCategory Category)
    {
        public bool Hide { get; init; }
    }

    public enum ClaimCategory
    {
        Protocol,
        Payload
    }

    public class TokenModel
    {
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public IEnumerable<TokenClaim> Claims { get; set; }
        public int ExpiresIn { get; set; }
        public string Subject { get; set; }
    }

    public record TokenClaim(string Type, string Value)
    {
        public ClaimCategory Category { get; init; }
    }
}
