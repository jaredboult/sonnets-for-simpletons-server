using Microsoft.AspNetCore.SignalR;
using SonnetsForSimpletonsServer.Lobby.Responses;
using SonnetsForSimpletonsServer.Player;
using SonnetsForSimpletonsServer.Player.Responses;

namespace SonnetsForSimpletonsServer.Lobby;

public class LobbyHub : Hub<ILobbyClient>
{
    private readonly IRoomFacade _roomFacade;
    private readonly IPlayerFacade _playerFacade;

    public LobbyHub(IRoomFacade roomFacade, IPlayerFacade playerFacade)
    {
        _roomFacade = roomFacade;
        _playerFacade = playerFacade;
    }

    public async Task<RoomResponse> CreateRoom()
    {
        var response = new RoomResponse();
        try
        {
            var player = _playerFacade.CreatePlayer(Context.ConnectionId);
            var room = _roomFacade.CreateRoom(player);
            
            response.Success = true;
            response.Description = $"Room {response.RoomId} was created";
            response.RoomId = room.RoomCode;
            
            await Groups.AddToGroupAsync(Context.ConnectionId, room.RoomCode);
        }
        catch (ApplicationException ex)
        {
            response.Success = false;
            response.Description = ex.Message == "Room limit reached"
                ? "Sorry, all rooms are full, try again later"
                : "An error occurred while creating a room";
        }
        return response;
    }

    public async Task<RoomResponse> JoinRoom(string roomId)
    {
        var response = new RoomResponse();
        var joiner = _playerFacade.CreatePlayer(Context.ConnectionId);
        if (_roomFacade.JoinRoom(joiner, roomId))
        {
            response.Success = true;
            response.Description = "Room joined";
            response.RoomId = roomId;
            await UpdateRoomDetails(roomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }
        else
        {
            response.Success = false;
            response.Description = "Room does not exist";
        }
        return response;
    }

    public async Task<PlayerResponse> UpdatePlayerName(string name, string roomId)
    {
        var response = new PlayerResponse();
        try
        {
            var validatedName = _playerFacade.UpdatePlayerName(name, Context.ConnectionId);
            if (validatedName is not null)
            {
                response.Success = true;
                response.Name = validatedName;
                response.Description = "Name was set to " + validatedName;
                await UpdateRoomDetails(roomId);
            }
            else
            {
                response.Success = false;
                response.Description = "Choose a different name";
            }

        }
        catch (ApplicationException ex)
        {
            response.Success = false;
            response.Description = ex.Message;
        }

        return response;
    }

    public RoomResponse GetRoomDetails(string roomId)
    {
        var response = GetRoomResponse(roomId);
        response.Description = "Getting the room details for single player";
        return response;
    }

    private async Task UpdateRoomDetails(string groupName)
    {
        // Right now the only group names are roomIds,
        // I will need to add validation here if that stops being true
        var response = GetRoomResponse(groupName);
        response.Description = "Updating the room details for all players, groupName: " + groupName;
        await Clients.Group(groupName).UpdateRoomDetails(response);
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
                .Select(player => player.Name)
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