using System.Threading.Tasks;

namespace Boost
{
    public interface IWebShell
    {
        Task<int> ExecuteAsync(ShellCommand command);
        Task<int> ExecuteAsync(params ShellCommand[] commands);
        void WriteLine(string value);
    }
}
