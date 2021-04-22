using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Boost.Infrastructure
{
    public static class NetworkExtensions
    {
        public static int GetAvailablePort(int startingPort)
        {
            if (startingPort > ushort.MaxValue)
                throw new ArgumentException(
                    $"Can't be greater than {ushort.MaxValue}",
                    nameof(startingPort));

            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            IEnumerable<IPEndPoint>? connectionsEndpoints = ipGlobalProperties.GetActiveTcpConnections()
                .Select(c => c.LocalEndPoint);
            IPEndPoint[]? tcpListenersEndpoints = ipGlobalProperties.GetActiveTcpListeners();
            IPEndPoint[] udpListenersEndpoints = ipGlobalProperties.GetActiveUdpListeners();
            IEnumerable<int> portsInUse = connectionsEndpoints.Concat(tcpListenersEndpoints)
                                                 .Concat(udpListenersEndpoints)
                                                 .Select(e => e.Port);

            return Enumerable.Range(startingPort, ushort.MaxValue - startingPort + 1)
                .Except(portsInUse)
                .FirstOrDefault();
        }
    }
}
