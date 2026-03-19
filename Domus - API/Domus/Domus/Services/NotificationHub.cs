using Microsoft.AspNetCore.SignalR;

namespace Domus.Services
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("Receive",message);
        }

        public async Task UpdateData()
        {
            await Clients.All.SendAsync("UpdateData");
        } 
    }
}
