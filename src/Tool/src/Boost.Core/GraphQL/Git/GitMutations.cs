using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Git;
using Boost.GraphQL;
using Boost.Settings;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.SignalR;

namespace Boost.Core.GraphQL;

[ExtendObjectType(RootTypes.Mutation)]
public partial class GitMutations
{
    public async Task<CloneGitRepositoryPayload> CloneRepositoryAsync(
        [Service] IWebShellFactory webShellFactory,
        CloneGitRepositoryInput input)
    {
        IWebShell shell = webShellFactory.CreateShell();

        var result = await shell.ExecuteGitAsync(
            new[] { "clone", input.Url },
            input.Directory);

        var gitPath = input.Url.Split('/').Last().Replace(".git", "");
        var dir = System.IO.Path.Combine(input.Directory, gitPath);

        return new CloneGitRepositoryPayload(result, dir);
    }

    public async Task<int> IndexWorkRootsAsync(
        [Service] IHubContext<ConsoleHub> hubContext,
        [Service] ILocalRepositoryIndexer repositoryIndexer,
        [Service] IUserSettingsManager settingsManager,
        CancellationToken cancellationToken)
    {
        UserSettings settings = await settingsManager.GetAsync(cancellationToken);

        var session = new MessageSession();

        Action<string> handler = async (string msg) =>
        {
            await hubContext.Clients.All.SendAsync("consoleData",
                new ShellMessage(session.Next(), "Info", msg));
        };

        int indexCount = 0;

        try
        {
            foreach (WorkRoot wr in settings.WorkRoots)
            {
                var count = await repositoryIndexer.IndexWorkRootAsync(
                    wr,
                    handler,
                    cancellationToken);

                indexCount += count;
            }

            await hubContext.Clients.All.SendAsync("consoleData",
                new ShellMessage(session.Next(), "end", $"Scan completed: {indexCount}")
                {
                    Tags = new string[] { "success" }
                });
        }
        catch (Exception ex)
        {
            await hubContext.Clients.All.SendAsync("consoleData",
                new ShellMessage(session.Next(), "error", ex.Message) { Tags = new[] { "error" } });
        }

        return indexCount;
    }
}
