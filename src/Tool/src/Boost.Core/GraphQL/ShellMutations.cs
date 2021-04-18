using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.GraphQL
{
    [ExtendObjectType(Name = RootTypes.Mutation)]
    public class ShellMutations
    {
        public Task<int> ExecuteCommandAsync(
            [Service] IWebShellFactory webShellFactory,
            ExecuteCommandInput input)
        {
            IWebShell shell = webShellFactory.CreateShell(input.Shell ?? "cmd");

            return shell.ExecuteAsync(new ShellCommand(input.Command)
            {
                Arguments = input.Arguments,
                WorkDirectory = input.WorkDirectory
            });
        }
    }

    public record ExecuteCommandInput(string Command)
    {
        public string? Arguments { get; init; }
        public string? WorkDirectory { get; init; }
        public string? Shell { get; init; }
    }
}
