using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus.Administration;
using Boost.AzureServiceBus.Models;
using Boost.AzureServiceBus.Services;
using Boost.Commands;
using ConsoleTables;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.AzureServiceBus.Commands
{
    [Command(
        Name = "asb",
        FullName = "AzureServiceBus",
        Description = "AzureServiceBus utils"), HelpOption]
    public class AzureServiceBusCommand : CommandBase
    {
        private readonly IAzureServiceBusService _azureServiceBusService;

        public AzureServiceBusCommand(IAzureServiceBusService azureServiceBusService)
        {
            _azureServiceBusService = azureServiceBusService;
        }

        [Argument(0, "action", ShowInHelpText = true, Description = "list|info")]
        public string? Action { get; set; } = "info";

        public void OnExecute(IConsole console)
        {
            switch (Action)
            {
                case "list":
                    IReadOnlyList<QueueInfo> queues = _azureServiceBusService.GetQueuesAsync(default).Result;

                    var table = new ConsoleTable("Name", "(Active, Deadletters)", "Status");

                    foreach (QueueInfo queue in queues)
                    {
                        table.AddRow(queue.Name, $"({queue.ActiveMessagesCount}, {queue.DeadletterMessagesCount})", queue.Status);
                    }

                    console.Write(table);

                    break;
                default:
                    console.WriteLine("Type info to list all queues.");
                    break;
            }
        }
    }
}
