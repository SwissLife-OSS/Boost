using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.AzureServiceBus.GraphQL
{
    public class AzureServiceBusPayload
    {
        public AzureServiceBusPayload(bool success, string? message=null)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get;}
        public string? Message { get; }
    }
}
