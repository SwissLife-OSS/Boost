using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.Settings;
using Serilog;

namespace Boost.Git;

public class GitRemoteSearchService : IGitRemoteSearchService
{
    private readonly IEnumerable<IGitRemoteClient> _clients;
    private readonly IConnectedServiceManager _connectedServiceManager;

    public GitRemoteSearchService(
        IEnumerable<IGitRemoteClient> clients,
        IConnectedServiceManager connectedServiceManager)
    {
        _clients = clients;
        _connectedServiceManager = connectedServiceManager;
    }

    public async Task<IEnumerable<GitRemoteRepository>> SearchAsync(
        SearchGitRepositoryRequest request,
        CancellationToken cancellationToken)
    {
        IEnumerable<IConnectedService> services = await _connectedServiceManager
            .GetServicesByFeatureAsync(
                ConnectedServiceFeature.GitRemoteRepository,
                cancellationToken);

        if (request.Services is { } s && s.Any())
        {
            services = services.Where(x => s.Contains(x.Type));
        }

        var repositories = new List<GitRemoteRepository>();

        var searchTasks = new List<Task<IEnumerable<GitRemoteRepository>>>();

        foreach (IConnectedService service in services)
        {
            IGitRemoteClient client = _clients
                .Single(x => x.ConnectedServiceType == service.Type);

            searchTasks.Add(client.SearchAsync(
                service.Id,
                request.Term,
                cancellationToken));
        }

        try
        {
            await Task.WhenAll(searchTasks);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Search Repository error");
        }

        return searchTasks
            .Where(x => x.IsCompletedSuccessfully)
            .SelectMany(x => x.Result);
    }
}
