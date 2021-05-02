using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.Web
{
    public interface IWebServer
    {
        public Task StartAsync(int port, string? path = null);
        Task StopAsync();
    }
}
