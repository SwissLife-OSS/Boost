using System;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.Commands;

[HelpOption]
public abstract class CommandBase
{
    public CancellationToken CommandAborded
        => new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
}
