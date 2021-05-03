using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.Git;
using Boost.Settings;
using CliWrap;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.Commands
{
    [Command(
        Name = "cr",
        FullName = "Clone Repository",
        Description = "Clone Repository"), HelpOption]
    public class CloneRepositoryCommand : CommandBase
    {
        private readonly IGitRemoteSearchService _searchService;
        private readonly IConnectedServiceManager _serviceManager;
        private readonly IUserSettingsManager _settingsManager;
        private readonly IDefaultShellService _shellService;
        private readonly ILocalRepositoryIndexer _localRepositoryIndexer;
        private WorkrootCommandUtils _wsUtils;

        public CloneRepositoryCommand(
            IGitRemoteSearchService searchService,
            IConnectedServiceManager serviceManager,
            IUserSettingsManager settingsManager,
            IDefaultShellService defaultShellService,
            ILocalRepositoryIndexer localRepositoryIndexer)
        {
            _searchService = searchService;
            _serviceManager = serviceManager;
            _settingsManager = settingsManager;
            _shellService = defaultShellService;
            _localRepositoryIndexer = localRepositoryIndexer;
        }

        [Argument(0, "filter", ShowInHelpText = true)]
        public string Filter { get; set; } = default!;

        [Option("--source|-s", Description = "Connected service name")]
        public string? Source { get; set; }

        public async Task OnExecute(CommandLineApplication app, IConsole console)
        {
            _wsUtils = new WorkrootCommandUtils(app, console);
            await _wsUtils.GetWorkRootsAsync(CommandAborded);

            var services = new List<string>();
            if (Source is { })
            {
                services.Add(Source);
            }

            GitRemoteRepository[] repos = (await _searchService.SearchAsync(
                new SearchGitRepositoryRequest(Filter)
                {
                    Services = services
                }, CommandAborded))
                .ToArray();

            if (repos.Length > 1)
            {
                var index = console.ChooseFromList(repos.Select(x => $"{x.FullName} ({x.Source})"));

                await CloneRepositoryAsync(console, repos[index]);
            }
            else if (repos.Length == 1)
            {
                await CloneRepositoryAsync(console, repos[0]);
            }
            else
            {
                console.WriteLine($"Not repository found with term: {Filter}", ConsoleColor.Yellow);
            }
        }

        private async Task CloneRepositoryAsync(IConsole console, GitRemoteRepository repository)
        {
            const string cloneCommand = "git clone";

            WorkRoot? workroot = await GetWorkRoot(repository);

            if (workroot is null)
            {
                console.WriteLine(
                    "No workroot could be found to clone repository, " +
                    "please configure your workroots in the UI", ConsoleColor.Red);

                return;
            }

            //Check if repo allready cloned
            string path = Path.Combine(workroot.Path, repository.Name);

            if (Directory.Exists(path))
            {
                console.WriteLine($"Repo allready cloned in: {path}.");
                console.WriteLine();

                await _wsUtils.ShowQuickActions(path);

                return;
            }

            var resultValidation = new CommandResultValidation();

            Command cmd = Cli.Wrap(_shellService.GetDefault())
                .WithArguments("/c " + $"{cloneCommand} {repository.WebUrl}")
                .WithWorkingDirectory(workroot.Path)
                .WithValidation(resultValidation)
                .WithStandardOutputPipe(PipeTarget.ToDelegate(message => console.WriteLine(message)))
                .WithStandardErrorPipe(PipeTarget.ToDelegate(message => console.WriteLine(message)));

            await cmd.ExecuteAsync();

            console.WriteLine($"Cloned {repository.Name} to {path}");
            await _localRepositoryIndexer.IndexRepository(workroot, path, CommandAborded);

            await _wsUtils.ShowQuickActions(path);
        }

        private async Task<WorkRoot?> GetWorkRoot(GitRemoteRepository repository)
        {
            ConnectedService? service = await _serviceManager.GetServiceAsync(
                repository.ServiceId,
                CommandAborded);

            return await _settingsManager
                .GetWorkRootAsync(service?.DefaultWorkRoot, CommandAborded);
        }

        private void DataRecevied(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
