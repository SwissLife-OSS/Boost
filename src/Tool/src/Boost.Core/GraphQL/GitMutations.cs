using System.Linq;
using System.Threading.Tasks;
using Boost.GraphQL;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.Core.GraphQL
{
    [ExtendObjectType(Name = RootTypes.Mutation)]
    public class GitMutations
    {
        public async Task<CloneGitRepositoryPayload> CloneRepositoryAsync(
            [Service] IWebShellFactory webShellFactory,
            CloneGitRepositoryInput input)
        {
            IWebShell shell = webShellFactory.CreateShell();

            var result = await shell.ExecuteAsync(new ShellCommand("git")
            {
                Arguments = $"clone {input.Url}",
                WorkDirectory = input.Directory
            });

            var gitPath = input.Url.Split('/').Last().Replace(".git", "");
            var dir = System.IO.Path.Combine(input.Directory, gitPath);

            return new CloneGitRepositoryPayload(result, dir);
        }
    }
}
