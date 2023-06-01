using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boost.Commands;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.GitHub.Commands;

[Command(
    Name = "ghauth",
    FullName = "GitHub Auth",
    Description = "Authenticate to GitHub"), HelpOption]
public class GitHubAuthCommand : CommandBase
{
    private readonly IGitHubAuthServer _gitHubAuthServer;
    private readonly IAuthServerServiceConfigurator _authServerServiceConfigurator;

    public GitHubAuthCommand(
        IGitHubAuthServer gitHubAuthServer,
        IAuthServerServiceConfigurator authServerServiceConfigurator)
    {
        _gitHubAuthServer = gitHubAuthServer;
        _authServerServiceConfigurator = authServerServiceConfigurator;
    }

    public async Task OnExecute(IConsole console)
    {
        await _gitHubAuthServer.StartAsync(3009, Guid.NewGuid(), (services) =>
        {
            _authServerServiceConfigurator.Configure(services);
        });
    }
}
