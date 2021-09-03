using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.AzureServiceBus.Settings
{
    public class ServiceBusConnection
    {
        public ServiceBusConnection(string name, string connectionString)
        {
            Name = name;
            ConnectionString = connectionString;
        }

        public string Name { get; }
        public string ConnectionString { get; }
    }
}
