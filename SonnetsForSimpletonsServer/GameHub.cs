using Microsoft.AspNetCore.SignalR;

namespace SonnetsForSimpletonsServer;

public class GameHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("ReceiveMessage", Context.UserIdentifier, "Hello");
    }
}