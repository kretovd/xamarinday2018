using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Xamarinday2018.Azure
{
    public class XamarinDay : Hub
    {
        public async Task Send(string color)
        {
            await Clients.All.InvokeAsync("Get", color);
        }
    }
}
