using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.Web
{
    public interface IWebServer : IDisposable
    {
        string LogLevel { get; set; }

        public Task<string> StartAsync(int port);
        Task StopAsync();
    }
}
