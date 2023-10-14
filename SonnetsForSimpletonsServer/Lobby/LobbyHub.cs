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

            await SendNewPlayerId(player.Id.ToString());
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

    private async Task SendNewPlayerId(string guid)
    {
        await Clients.Caller.SavePlayerId(new PlayerResponse
        {
            Success = true,
            Description = "Store this unique id to persist your identity",
            Id = guid
        });
    }

    public async Task<RoomResponse> JoinRoom(string roomId)
    {
        var response = new RoomResponse();
        var joiner = _playerFacade.CreatePlayer(Context.ConnectionId);
        await SendNewPlayerId(joiner.Id.ToString());
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

    public async Task<PlayerResponse> UpdatePlayerName(string name, string roomId, string id)
    {
        var response = new PlayerResponse();
        try
        {
            var validatedName = _playerFacade.UpdatePlayerName(name, id);
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

    public async Task<GeneralResponse> StartGameForRoom(string roomId, string playerGuid)
    {
        var response = new GeneralResponse();
        var room = _roomFacade.GetRoom(roomId);
        var player = _playerFacade.GetPlayer(playerGuid);
        if (room is null || player is null || !room.Players.Contains(player))
        {
            response.Success = false;
            response.Description = "Unable to start the game";
            
        }
        else
        {
            response.Success = true;
            response.Description = "Starting the game";
            await Clients.OthersInGroup(roomId).StartGame(response);
        }
        return response;
    }
}