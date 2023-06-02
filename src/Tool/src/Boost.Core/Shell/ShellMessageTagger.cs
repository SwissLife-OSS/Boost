using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Boost;

public static class ShellMessageTagger
{
    static Dictionary<string, string> _tags;

    static ShellMessageTagger()
    {
        _tags = new()
        {
            ["error"] = ".*error.*",
            ["warning"] = ".*warning.*",
            ["success"] = ".*success.*|.*finished.*|.*completed.*"
        };
    }

    public static IEnumerable<string> GetTags(string? message)
    {
        if (message != null)
        {
            foreach (KeyValuePair<string, string> tagMatch in _tags)
            {
                if (Regex.IsMatch(message, tagMatch.Value, RegexOptions.IgnoreCase))
                {
                    yield return tagMatch.Key;
                }
            }
        }
    }
}
