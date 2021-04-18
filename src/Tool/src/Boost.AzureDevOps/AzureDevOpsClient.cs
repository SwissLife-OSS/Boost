using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Core.WebApi;

namespace Boost.AzureDevOps
{
    public class AzureDevOpsClient
    {
        public AzureDevOpsClient(
            AzureDevOpsClientFactory azureDevOpsClientFactory)
        {
            ClientFactory = azureDevOpsClientFactory;
        }

        protected AzureDevOpsClientFactory ClientFactory { get; }

        protected string DefaultTeamProject => throw new NotSupportedException();

        public async Task<TeamProject> GetTeamProjectAsync(
            Guid serviceId,
            string nameOrId,
            CancellationToken cancellationToken)
        {
            await ClientFactory.ConnectAsync(serviceId, cancellationToken);

            ProjectHttpClient client = ClientFactory.CreateClient<ProjectHttpClient>(serviceId);

            return await client.GetProject(nameOrId);
        }
    }
}
