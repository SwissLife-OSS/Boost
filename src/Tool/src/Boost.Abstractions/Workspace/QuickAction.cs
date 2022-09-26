using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Boost.Workspace
{
    public class QuickAction
    {
        public string Type { get; set; } = default!;

        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public string Value { get; set; } = default!;

        public IEnumerable<string> Tags { get; set; } = new List<string>();

        public override string ToString()
        {
            switch (Type)
            {
                case QuickActionTypes.RunSuperBoost:
                    return $"SuperBoost: {Title}";
                default:
                    return Description;
            }
        }
    }
}
