using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.AzureDevOps
{
    public interface IAzureDevOpsPersonalAccessTokenResolver
    {
        public string GetTokenAsync(CancellationToken cancellationToken);
    }
}
