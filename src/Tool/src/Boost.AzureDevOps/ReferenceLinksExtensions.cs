using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Services.WebApi;

namespace Boost.AzureDevOps
{
    public static class ReferenceLinksExtensions
    {
        public static string? GetLink(this ReferenceLinks links, string name)
        {
            if (links is null)
            {
                return null;
            }

            KeyValuePair<string, object> link = links.Links.FirstOrDefault(x => x.Key == name);

            if (link.Value is ReferenceLink rl)
            {
                return rl.Href;
            }

            return null;
        }
    }
}
