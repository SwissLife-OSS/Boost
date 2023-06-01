using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Boost;

public class ConsoleHub : Hub
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }
}
