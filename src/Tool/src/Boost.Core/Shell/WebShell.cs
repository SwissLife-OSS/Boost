using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Boost.Infrastructure;

namespace Boost
{
    public class WebShell : IWebShell
    {
        private readonly Action<ShellMessage> _messageHandler;
        private readonly IBoostApplicationContext _boostApplicationContext;
        private ProcessStartInfo _startInfo = new ProcessStartInfo();
        private readonly MessageSession _session;


        public WebShell(
            string shell,
            Action<ShellMessage> messageHandler,
            IBoostApplicationContext boostApplicationContext)
        {
            _messageHandler = messageHandler;
            _boostApplicationContext = boostApplicationContext;
            CreateStartInfo(shell);
            _session = new();
        }

        private void CreateStartInfo(string type)
        {
            _startInfo.CreateNoWindow = true;
            _startInfo.RedirectStandardOutput = true;
            _startInfo.RedirectStandardError = true;
            _startInfo.RedirectStandardInput = true;
            _startInfo.UseShellExecute = false;
            _startInfo.FileName = type;
        }

        public async Task<int> ExecuteAsync(ShellCommand command)
        {
            _startInfo.Arguments = $"/c {command.Command} {command.Arguments}";
            _messageHandler?.Invoke(new ShellMessage(
                _session.Next(),
                "cmd",
                $"{_startInfo.FileName}> {command.Command} {command.Arguments}")
            {
                Tags = new[] { "command" }
            });

            var process = new Process();
            process.StartInfo = _startInfo;
            process.StartInfo.WorkingDirectory = command.WorkDirectory ??
                _boostApplicationContext.WorkingDirectory.FullName;

            process.EnableRaisingEvents = true;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorOutputDataReceived;
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            _messageHandler?.Invoke(new ShellMessage(_session, "start", ""));
            await process.WaitForExitAsync();;

            _messageHandler?.Invoke(new ShellMessage(
                 _session.Next(),
                "end",
                (process.ExitCode == 0) ?
                "Command completed successfully" :
                $"Command completed with errors (ExitCode: {process.ExitCode})"
                )
            {
                Tags = new string[] { (process.ExitCode == 0) ? "success" : "error" }
            });

            return process.ExitCode;
        }

        public async Task<int> ExecuteAsync(params ShellCommand[] commands)
        {
            int exitCode = 0;

            foreach (ShellCommand cmd in commands)
            {
                exitCode = await ExecuteAsync(cmd);
                if (exitCode != 0)
                {
                    break;
                }
            }

            return exitCode;
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _messageHandler?.Invoke(new ShellMessage(_session.Next(), "info", e.Data)
            {
                Tags = ShellMessageTagger.GetTags(e.Data)
            });
        }

        private void Process_ErrorOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _messageHandler?.Invoke(new ShellMessage(_session.Next(), "error", e.Data)
            {
                Tags = new[] { "error" }
            });
        }
    }
}
