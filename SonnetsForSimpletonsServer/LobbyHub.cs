using Microsoft.AspNetCore.SignalR;
using SonnetsForSimpletonsServer.Models.Messages;

namespace SonnetsForSimpletonsServer;

public class LobbyHub : Hub
{
    private const string GeneratedRoomCode = "ABCD";
    private readonly IRoomFacade _roomFacade;

    public LobbyHub(IRoomFacade roomFacade)
    {
        _roomFacade = roomFacade;
    }
    
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("ReceiveMessage", $"{DateTime.Now} - A new client has connected");
    }

    public async Task CreateRoom()
    {
        var response = new RoomResponse();
        try
        {
            var room = _roomFacade.CreateRoom();
            response.RoomId = room.RoomCode;
            response.Success = true;
        }
        catch (ApplicationException ex)
        {
            response.Success = false;
            response.Description = ex.Message == "Room limit reached" 
                ? "Sorry, all rooms are full, try again later"
                : "An error occurred while creating a room";
        }
        await Clients.Caller.SendAsync("CreateRoom", response);
    }

    public async Task JoinRoom(string roomId)
    {
        var response = new RoomResponse();
        if (_roomFacade.JoinRoom(roomId))
        {
            response.Success = true;
            response.Description = "Room joined";
            response.RoomId = roomId;
        }
        else
        {
            response.Success = false;
            response.Description = "Invalid room code";
        }
        await Clients.Caller.SendAsync("JoinRoom", response);
    }
}