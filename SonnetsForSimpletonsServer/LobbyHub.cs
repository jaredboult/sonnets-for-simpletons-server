using Microsoft.AspNetCore.SignalR;
using SonnetsForSimpletonsServer.Models.Messages;

namespace SonnetsForSimpletonsServer;

public class LobbyHub : Hub<ILobbyHub>
{
    private readonly IRoomFacade _roomFacade;
    private readonly IPlayerFacade _playerFacade;

    public LobbyHub(IRoomFacade roomFacade, IPlayerFacade playerFacade)
    {
        _roomFacade = roomFacade;
        _playerFacade = playerFacade;
    }

    public async Task CreateRoom()
    {
        var response = new RoomResponse();
        try
        {
            var player = _playerFacade.CreatePlayer(Context.ConnectionId);
            var room = _roomFacade.CreateRoom(player);
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

        await Clients.Caller.CreateRoom(response);
    }

    public async Task JoinRoom(string roomId)
    {
        var response = new RoomResponse();
        var joiner = _playerFacade.CreatePlayer(Context.ConnectionId);
        if (_roomFacade.JoinRoom(joiner, roomId))
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

        await Clients.Caller.JoinRoom(response);
    }

    public async Task UpdatePlayerName(string name)
    {
        var response = new PlayerResponse();
        try
        {
            _playerFacade.UpdatePlayerName(name, Context.ConnectionId);
            response.Success = true;
            response.Name = name;
        }
        catch (ApplicationException ex)
        {
            response.Success = false;
            response.Description = ex.Message;
        }

        await Clients.Caller.UpdatePlayerName(response);
    }
}