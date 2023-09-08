using Microsoft.AspNetCore.SignalR;
using SonnetsForSimpletonsServer.Models.LobbyResponses;

namespace SonnetsForSimpletonsServer;

public class LobbyHub : Hub
{
    private const string GeneratedRoomId = "ABCD";
    
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("ReceiveMessage", $"{DateTime.Now} - A new client has connected");
    }

    public async Task CreateRoom()
    {
        await Clients.Caller.SendAsync("CreateRoom", GeneratedRoomId);
    }

    public async Task JoinRoom(string roomId)
    {
        var response = new JoinRoomResponse();
        if (roomId == GeneratedRoomId)
        {
            response.Success = true;
            response.Description = "Room joined";
            response.RoomId = GeneratedRoomId;
        }
        else
        {
            response.Success = false;
            response.Description = "Invalid room code   ";
        }
        await Clients.Caller.SendAsync("JoinRoom", response);
    }
}