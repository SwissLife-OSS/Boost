using System.Collections.Generic;

namespace Boost.AzureServiceBus.Settings
{
    public class AzureServiceBusSettings
    {
        public List<AzureServiceBusConnection> Connections { get; set; } = new List<AzureServiceBusConnection>();

        public void AddOrReplaceConnection(AzureServiceBusConnection serviceBusConnection)
        {
            int index = Connections.FindIndex(
                p => p.Name.ToLowerInvariant().Equals(
                    serviceBusConnection.Name.ToLowerInvariant()));

            if (index == -1)
            {
                Connections.Add(serviceBusConnection);
            }
            else
            {
                Connections[index] = serviceBusConnection;
            }
        }
    }
}
