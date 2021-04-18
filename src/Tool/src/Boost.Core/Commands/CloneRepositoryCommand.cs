using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Boost.Core.Settings;
using Boost.Git;
using Boost.Settings;
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

        public CloneRepositoryCommand(
            IGitRemoteSearchService searchService,
            IConnectedServiceManager serviceManager,
            IUserSettingsManager settingsManager)
        {
            _searchService = searchService;
            _serviceManager = serviceManager;
            _settingsManager = settingsManager;
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
            var path = await GetWorkRoot(repository);

            if (path is null)
            {
                console.WriteLine(
                    "No workroot could be found to clone repository, " +
                    "please configure your workroots in the UI", ConsoleColor.Red);

                return;
            }

            console.WriteLine($"Start cloning {repository.Name}... into: {path}", ConsoleColor.Blue);
        }

        private async Task<string?> GetWorkRoot(GitRemoteRepository repository)
        {
            ConnectedService? service = await _serviceManager.GetServiceAsync(
                repository.ServiceId,
                CommandAborded);

            return await _settingsManager
                .GetWorkRootAsync(service?.DefaultWorkRoot, CommandAborded);
        }

        private IEnumerable<string> RunGitCommand(string directory, params string[] commands)
        {
            var results = new List<string>();

            foreach (var cmd in commands)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.FileName = @"git.exe";
                startInfo.Arguments = cmd;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.WorkingDirectory = directory;

                Process? gitProcess = Process.Start(startInfo);

                if (gitProcess is null)
                {
                    throw new ApplicationException("Process is null");
                }

                gitProcess.OutputDataReceived += DataRecevied;
                gitProcess.ErrorDataReceived += DataRecevied;

                gitProcess.BeginOutputReadLine();
                gitProcess.BeginErrorReadLine();

                gitProcess.WaitForExit();
                if (gitProcess.ExitCode != 0)
                {
                    throw new ApplicationException($"ExitCode {gitProcess.ExitCode}");
                }
                gitProcess.Close();
            }
            return results;
        }

        private void DataRecevied(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
