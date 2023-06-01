using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.GraphQL
{
    [ExtendObjectType(RootTypes.Mutation)]
    public class ShellMutations
    {
        public Task<int> ExecuteCommandAsync(
            [Service] IWebShellFactory webShellFactory,
            ExecuteCommandInput input)
        {
            IWebShell shell = webShellFactory.CreateShell();

            return shell.ExecuteShellAsync(input.Command, input.WorkDirectory);
        }
    }
}
