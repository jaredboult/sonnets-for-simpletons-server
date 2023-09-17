using Microsoft.AspNetCore.SignalR;
using SonnetsForSimpletonsServer.Models.Messages;

namespace SonnetsForSimpletonsServer;

public class LobbyHub : Hub<ILobbyClient>
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
            await Groups.AddToGroupAsync(Context.ConnectionId, room.RoomCode);
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
            response.Description = "Room does not exist";
        }
        await Clients.Caller.JoinRoom(response);
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await UpdateRoomDetails(roomId);
    }

    public async Task UpdatePlayerName(string name, string roomId)
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
        await UpdateRoomDetails(roomId);
    }

    public async Task GetRoomDetails(string roomId)
    {
        var response = GetRoomResponse(roomId);
        await Clients.Caller.ReceiveRoomDetails(response);
    }

    public async Task UpdateRoomDetails(string groupName)
    {
        // Right now the only group names are roomIds,
        // I will need to add validation here if that stops being true
        var response = GetRoomResponse(groupName);
        await Clients.Group(groupName).ReceiveRoomDetails(response);
    }

    private RoomResponse GetRoomResponse(string roomId)
    {
        var response = new RoomResponse();
        var room = _roomFacade.GetRoom(roomId);
        if (room is not null)
        {
            response.Success = true;
            response.RoomId = roomId;
            response.PlayerNames = room.Players
                .Select(p => p.Name)
                .ToList();
        }
        else
        {
            response.Success = false;
            response.Description = "Room does not exist";
        }

        return response;
    }
}