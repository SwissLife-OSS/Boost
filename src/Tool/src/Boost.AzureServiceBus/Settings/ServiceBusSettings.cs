using System.Collections.Generic;

namespace Boost.AzureServiceBus.Settings
{
    public class ServiceBusSettings
    {
        public ServiceBusSettings()
        {
            Connections = new List<ServiceBusConnection>();
        }

        public ServiceBusSettings(List<ServiceBusConnection> connections)
        {
            Connections = connections;
        }
        
        public List<ServiceBusConnection> Connections { get; }

        public void AddOrReplaceConnection(ServiceBusConnection serviceBusConnection)
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
