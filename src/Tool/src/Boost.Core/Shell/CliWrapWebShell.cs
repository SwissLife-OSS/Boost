using System;
using System.Threading.Tasks;
using Boost.Infrastructure;
using CliWrap;
using CliWrap.EventStream;

namespace Boost.Shell
{
    public class CliWrapWebShell : IWebShell
    {
        private readonly string _shell;
        private readonly Action<ShellMessage> _messageHandler;
        private readonly IBoostApplicationContext _boostApplicationContext;
        private readonly MessageSession _session;

        public CliWrapWebShell(
            string shell,
            Action<ShellMessage> messageHandler,
            IBoostApplicationContext boostApplicationContext)
        {
            _shell = shell;
            _messageHandler = messageHandler;
            _boostApplicationContext = boostApplicationContext;
            _session = new();
        }

        public async Task<int> ExecuteAsync(ShellCommand command)
        {
            Command? cmd = Cli.Wrap(_shell)
                .WithArguments("/c " + command.Command + " " + command.Arguments)
                .WithWorkingDirectory(command.WorkDirectory ?? _boostApplicationContext.WorkingDirectory.FullName);

            var exitCode = 0;

            await foreach (CommandEvent? cmdEvent in cmd.ListenAsync())
            {
                switch (cmdEvent)
                {
                    case StartedCommandEvent started:
                        _messageHandler?.Invoke(new ShellMessage(
                            _session.Next(),
                            "cmd",
                            $"{_shell}> {command.Command} {command.Arguments}")
                        {
                            Tags = new[] { "command" }
                        });
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
                            (exitCode == 0) ?
                            "Command completed successfully" :
                            $"Command completed with errors (ExitCode: {exitCode})"
                            )
                        {
                            Tags = new string[] { (exitCode == 0) ? "success" : "error" }
                        });
                        break;
                }
            }

            return exitCode;
        }

        public async Task<int> ExecuteAsync(params ShellCommand[] commands)
        {
            foreach (ShellCommand cmd in commands)
            {
                int result = await ExecuteAsync(cmd);

                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }
    }
}
