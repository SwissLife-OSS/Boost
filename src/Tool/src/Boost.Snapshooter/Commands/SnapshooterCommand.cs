using System;
using System.Collections.Generic;
using System.Linq;
using Boost.Commands;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.Snapshooter.Commands;

[Command(
    Name = "snap",
    FullName = "Snapshooter",
    Description = "Snapshooter utils"), HelpOption]
public class SnapshooterCommand : CommandBase
{
    private readonly ISnapshooterService _snapshooterService;

    public SnapshooterCommand(ISnapshooterService snapshooterService)
    {
        _snapshooterService = snapshooterService;
    }

    [Argument(0, "action", ShowInHelpText = true, Description = "accept|info")]
    public string? Action { get; set; } = "info";

    public void OnExecute(IConsole console)
    {
        switch (Action)
        {
            case "accept":
                var approved = _snapshooterService.ApproveAllMismatches();
                console.WriteLine($"{approved} mismatched accepted");
                break;
            default:
                IEnumerable<SnapshotInfo> snapshots = _snapshooterService
                    .GetSnapshots(withMismatchOnly: false);

                if (snapshots.Count() == 0)
                {
                    console.WriteLine("No snapshots found.", ConsoleColor.Yellow);
                }
                else
                {
                    var mismatches = snapshots.Where(x => x.HasMismatch).ToList();

                    console.WriteLine("Snapshooter summary:");
                    console.WriteLine("--------------------");
                    console.WriteLine($"Total snapshots: {snapshots.Count()}");
                    console.WriteLine($"Total mismatches: {mismatches.Count()}",
                        mismatches.Count() == 0 ? ConsoleColor.Green : ConsoleColor.Red);

                    if (mismatches.Count() > 0)
                    {
                        console.WriteLine();
                        console.WriteLine("Mismatches");
                        console.WriteLine("-----------");

                        foreach (SnapshotInfo mm in mismatches)
                        {
                            console.WriteLine($"{mm.Name} in {mm.Directory}");
                        }

                        console.WriteLine();
                        console.WriteLine("You can use `boo snap accept` to accept all mismatches.");

                        var approveAll = Prompt.GetYesNo(
                            "Approve all now?",
                            false);

                        if (approveAll)
                        {
                            _snapshooterService.ApproveAllMismatches();
                        }
                    }
                }

                break;
        }
    }
}
