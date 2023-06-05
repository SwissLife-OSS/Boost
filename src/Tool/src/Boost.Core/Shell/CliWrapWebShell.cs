using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Infrastructure;
using CliWrap;
using CliWrap.EventStream;

namespace Boost.Shell;

public class CliWrapWebShell : IWebShell
{
    private readonly string _shell;
    private readonly Action<ShellMessage> _messageHandler;
    private readonly IToolManager _toolManager;
    private readonly IBoostApplicationContext _boostApplicationContext;
    private readonly MessageSession _session;

    public CliWrapWebShell(
        string shell,
        Action<ShellMessage> messageHandler,
        IToolManager toolManager,
        IBoostApplicationContext boostApplicationContext)
    {
        _shell = shell;
        _messageHandler = messageHandler;
        _toolManager = toolManager;
        _boostApplicationContext = boostApplicationContext;
        _session = new();
    }

    public async Task<int> ExecuteShellAsync(string arguments, string? workingDirectory)
    {
        var shellPath = await _toolManager.GetToolPathAsync(_shell, CancellationToken.None);

        Command? cmd = Cli.Wrap(shellPath)
            .WithArguments("-c " + arguments)
            .WithWorkingDirectory(workingDirectory ?? _boostApplicationContext.WorkingDirectory.FullName);

        return await ExecuteAsync(cmd);
    }

    public async Task<int> ExecuteGitAsync(IEnumerable<string> arguments, string directory)
    {
        Command? cmd = Cli.Wrap("git")
            .WithArguments(arguments)
            .WithWorkingDirectory(directory);

        return await ExecuteAsync(cmd);
    }

    public async Task<int> ExecuteGitManyAsync(IEnumerable<string[]> commands, string directory)
    {
        var result = 0;

        foreach (var command in commands)
        {
            result = await ExecuteGitAsync(command, directory);

            if (result > 0)
                return result;
        }

        return result;
    }

    public async Task<int> ExecuteAsync(string targetFilename, string arguments, string? workingDirectory)
    {
        Command? cmd = Cli.Wrap(targetFilename)
            .WithArguments(arguments)
            .WithWorkingDirectory(workingDirectory ?? _boostApplicationContext.WorkingDirectory.FullName);

        return await ExecuteAsync(cmd);
    }

    public async Task<int> ExecuteManyAsync(string targetFilename, string[] commands, string? workingDirectory)
    {
        var result = 0;

        foreach (var command in commands)
        {
            result = await ExecuteAsync(targetFilename, command, workingDirectory);

            if (result > 0)
                return result;
        }

        return result;
    }

    private async Task<int> ExecuteAsync(Command cmd)
    {
        var exitCode = 0;

        await foreach (CommandEvent? cmdEvent in cmd.ListenAsync())
        {
            switch (cmdEvent)
            {
                case StartedCommandEvent:
                    _messageHandler?.Invoke(new ShellMessage(
                        _session.Next(),
                        "cmd",
                        $"{_shell}> {cmd.Arguments}") { Tags = new[] { "command" } });
                    break;
                case StandardOutputCommandEvent stdOut:
                    _messageHandler?.Invoke(new ShellMessage(_session.Next(), "info", stdOut.Text)
                    {
                        Tags = ShellMessageTagger.GetTags(stdOut.Text)
                    });
                    break;
                case StandardErrorCommandEvent stdErr:
                    _messageHandler?.Invoke(new ShellMessage(_session.Next(), "error", stdErr.Text)
                    {
                        Tags = new[] { "error" }
                    });
                    break;
                case ExitedCommandEvent exited:
                    exitCode = exited.ExitCode;
                    _messageHandler?.Invoke(new ShellMessage(
                        _session.Next(),
                        "end",
                        (exitCode == 0)
                            ? "Command completed successfully"
                            : $"Command completed with errors (ExitCode: {exitCode})"
                    ) { Tags = new string[] { (exitCode == 0) ? "success" : "error" } });
                    break;
            }
        }

        return exitCode;
    }
}
