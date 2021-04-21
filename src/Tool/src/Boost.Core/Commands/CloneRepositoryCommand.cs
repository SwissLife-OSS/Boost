using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public CloneRepositoryCommand(
            IGitRemoteSearchService searchService,
            IConnectedServiceManager serviceManager,
            IUserSettingsManager settingsManager,
            IDefaultShellService defaultShellService)
        {
            _searchService = searchService;
            _serviceManager = serviceManager;
            _settingsManager = settingsManager;
            _shellService = defaultShellService;
        }

        [Argument(0, "filter", ShowInHelpText = true)]
        public string Filter { get; set; } = default!;

        [Option("--source|-s", Description = "Connected service name")]
        public string? Source { get; set; }

        public async Task OnExecute(IConsole console)
        {
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

            var path = await GetWorkRoot(repository);

            if (path is null)
            {
                console.WriteLine(
                    "No workroot could be found to clone repository, " +
                    "please configure your workroots in the UI", ConsoleColor.Red);

                return;
            }

            var resultValidation = new CommandResultValidation();

            Command cmd = Cli.Wrap(_shellService.GetDefault())
                .WithArguments("/c " + $"{cloneCommand} {repository.WebUrl}")
                .WithWorkingDirectory(path)
                .WithValidation(resultValidation)
                .WithStandardOutputPipe(PipeTarget.ToDelegate(message => console.WriteLine(message)))
                .WithStandardErrorPipe(PipeTarget.ToDelegate(message => console.WriteLine(message)));

            await cmd.ExecuteAsync();
        }

        private async Task<string?> GetWorkRoot(GitRemoteRepository repository)
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
