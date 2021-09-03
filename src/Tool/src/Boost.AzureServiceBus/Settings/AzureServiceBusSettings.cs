using System;
using System.Collections.Generic;
using System.Linq;

namespace Boost.AzureServiceBus.Settings
{
    public class AzureServiceBusSettings
    {
        public List<AzureServiceBusConnection> Connections { get; set; } = new List<AzureServiceBusConnection>();

        internal void AddOrReplaceConnection(AzureServiceBusConnection serviceBusConnection)
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

        internal void RemoveConnection(string connectionName)
        {
            AzureServiceBusConnection? connection = Connections.FirstOrDefault(p => p.Name
                .ToLowerInvariant()
                .Equals(connectionName.ToLowerInvariant()));

            if (connection != null)
            {
                Connections.Remove(connection);
            }
        }
    }
}
