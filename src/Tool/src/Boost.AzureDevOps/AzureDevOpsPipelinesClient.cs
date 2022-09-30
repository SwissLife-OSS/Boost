using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Pipelines;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Clients;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Contracts;
using Microsoft.VisualStudio.Services.WebApi;

namespace Boost.AzureDevOps
{
    public class AzureDevOpsPipelinesClient :
        AzureDevOpsClient,
        IAzureDevOpsPipelinesClient,
        IPipelinesClient
    {
        public string ConnectedServiceType => AzureDevOpsConstants.ServiceTypeName;

        public AzureDevOpsPipelinesClient(
            AzureDevOpsClientFactory azureDevOpsClientFactory)
            : base(azureDevOpsClientFactory)
        { }

        public async Task<IEnumerable<Pipeline>> GetPipelinesAsync(
            Guid serviceId,
            string repositoryId,
            CancellationToken cancellationToken)
        {
            IEnumerable<BuildDefinition> buildDefinitions = await GetPipelinesByRepositoryAsync(
                serviceId,
                Guid.Parse(repositoryId),
                cancellationToken);

            var pipelines = new List<Pipeline>();
            AzureDevOpsConnectedService service = ClientFactory.Connections[serviceId].Service;

            foreach (BuildDefinition defintion in buildDefinitions)
            {
                Pipeline pipeline = ToPipeline(service, defintion);

                Pipeline? releaseDef = await GetReleaseDefinitionAsync(
                    serviceId,
                    defintion.Project.Id,
                    defintion.Id,
                    cancellationToken: cancellationToken);

                if (releaseDef is { })
                {
                    pipeline.Children = new List<Pipeline> { releaseDef };
                }

                //pipeline.Children = await GetReleasesAsync(
                //    serviceId,
                //    defintion.Project.Id,
                //    defintion.Id,
                //    cancellationToken: cancellationToken);

                pipelines.Add(pipeline);
            }

            return pipelines;
        }

        public async Task<IEnumerable<PipelineRun>> GetRunsAsync(
            Pipeline pipeline,
            int top,
            CancellationToken cancellationToken)
        {
            var projectId = Guid.Parse(pipeline.Properties.First(x => x.Name == "ProjectId").Value);

            IEnumerable<Build>? runs = await GetRunsAsync(
                pipeline.ServiceId,
                projectId,
                int.Parse(pipeline.Id),
                top,
                cancellationToken);

            return runs.Select(x => ToRun(x, pipeline));
        }

        private async Task<IEnumerable<BuildDefinition>> GetPipelinesByRepositoryAsync(
            Guid serviceId,
            Guid repositoryId,
            CancellationToken cancellationToken)
        {
            AzureDevOpsConnectedService service = await ClientFactory.ConnectAsync(
                serviceId,
                cancellationToken);

            BuildHttpClient client = ClientFactory.CreateClient<BuildHttpClient>(serviceId);

            IPagedList<BuildDefinition> pipelines = await client
                .GetFullDefinitionsAsync2(
                    project: service.DefaultProject,
                    repositoryType: "TfsGit",
                    repositoryId: repositoryId.ToString(),
                    cancellationToken: cancellationToken);

            return pipelines.ToList();
        }

        private PipelineRun ToRun(Build run, Pipeline pipeline)
        {
            return new PipelineRun
            {
                Id = run.Id.ToString(),
                Name = run.BuildNumber,
                Title = $"{pipeline.Name}_{run.BuildNumber}",
                StartedAt = run.StartTime,
                FinishedAt = run.FinishTime,
                Status = run.Status?.ToString() ?? "Unknown",
                Result = run.Result?.ToString() ?? "Unknown",
                RequestedFor = run.RequestedFor.DisplayName,
                Reason = run.Reason.ToString(),
                WebLink = run.Links.GetLink("web"),
                ServiceId = pipeline.ServiceId
            };
        }

        private Pipeline ToPipeline(
            AzureDevOpsConnectedService service,
            BuildDefinition pipeline)
        {
            return new Pipeline
            {
                Id = pipeline.Id.ToString(),
                Type = "AzureDevOps_Pipeline",
                Name = pipeline.Name,
                ServiceId = service.Id,
                WebUrl = pipeline.Links.GetLink("web"),
                Properties = new List<PipelineProperty>
                {
                    new("ProjectId", pipeline.Project.Id.ToString())
                }
            };
        }

        private Pipeline ToPipeline(
            AzureDevOpsConnectedService service,
            Release release)
        {
            return new Pipeline
            {
                Id = release.Id.ToString(),
                Type = "AzureDevOps_Release",
                Name = release.Name,
                ServiceId = service.Id,
                WebUrl = release.Links.GetLink("web"),
                Properties = new List<PipelineProperty>
                {
                    new("ProjectId", release.ProjectReference.Id.ToString())
                }
            };
        }

        private Pipeline ToPipeline(
            AzureDevOpsConnectedService service,
            ReleaseDefinitionShallowReference definition)
        {
            return new Pipeline
            {
                Id = definition.Id.ToString(),
                Type = "AzureDevOps_ReleaseDefinition",
                Name = definition.Name,
                ServiceId = service.Id,
                WebUrl = definition.Links.GetLink("web"),
                Properties = new List<PipelineProperty>
                {
                    new("ProjectId", definition.ProjectReference.Id.ToString())
                }
            };
        }

        public async Task<IEnumerable<Pipeline>> GetReleasesAsync(
            Guid serviceId,
            Guid teamProjectId,
            int buildDefinitionId,
            int top = 10,
            CancellationToken cancellationToken = default)
        {
            ReleaseHttpClient client = ClientFactory.CreateClient<ReleaseHttpClient>(serviceId);

            List<Release> releases = await client.GetReleasesAsync(
                project: teamProjectId,
                artifactTypeId: "Build",
                sourceId: $"{teamProjectId}:{buildDefinitionId}",
                top: top,
                cancellationToken: cancellationToken);

            return releases.Select(x => ToPipeline(ClientFactory.Connections[serviceId].Service, x));
        }

        public async Task<Pipeline?> GetReleaseDefinitionAsync(
            Guid serviceId,
            Guid teamProjectId,
            int buildDefinitionId,
            CancellationToken cancellationToken = default)
        {
            ReleaseHttpClient client = ClientFactory.CreateClient<ReleaseHttpClient>(serviceId);

            List<Release> releases = await client.GetReleasesAsync(
                project: teamProjectId,
                artifactTypeId: "Build",
                sourceId: $"{teamProjectId}:{buildDefinitionId}",
                top: 1,
                cancellationToken: cancellationToken);

            if (releases.FirstOrDefault() is Release release)
            {
                ReleaseDefinitionShallowReference? definition = (ReleaseDefinitionShallowReference)typeof(Release)
                    .GetProperty("ReleaseDefinition")
                    .GetValue(release);

                if ( definition is { })
                {
                    definition.ProjectReference = release.ProjectReference;
                    return ToPipeline(ClientFactory.Connections[serviceId].Service, definition);
                }
            }

            return null;

        }

        public async Task<IEnumerable<Build>> GetRunsAsync(
            Guid serviceId,
            Guid projectId,
            int id,
            int top = 5,
            CancellationToken cancellationToken = default)
        {
            await ClientFactory.ConnectAsync(serviceId, cancellationToken);

            BuildHttpClient client = ClientFactory.CreateClient<BuildHttpClient>(serviceId);

            IPagedList<Build> builds = await client.GetBuildsAsync2(
                projectId,
                new List<int>() { id },
                top: top,
                cancellationToken: cancellationToken);

            return builds;
        }
    }

    public class AzureDevOpsRepositoryIdentity : IRepositoryIdentity
    {
        public Guid RepositorId { get; set; }

        public string? Project { get; set; }

        public string Id => RepositorId.ToString();
    }
}
