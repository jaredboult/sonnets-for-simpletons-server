using Microsoft.AspNetCore.SignalR;
using SonnetsForSimpletonsServer.Game.Responses;
using SonnetsForSimpletonsServer.Lobby;
using SonnetsForSimpletonsServer.Player;

namespace SonnetsForSimpletonsServer.Game;

public class SonnetsForSimpletonsHub : Hub<ISonnetsForSimpletonClient>
{
    private readonly IRoomFacade _roomFacade;
    private readonly IPlayerFacade _playerFacade;

    public SonnetsForSimpletonsHub(IRoomFacade roomFacade, IPlayerFacade playerFacade)
    {
        _roomFacade = roomFacade;
        _playerFacade = playerFacade;
    }

    public GameResponse StartGame(string roomId)
    {
        var response = new GameResponse();
        var player = _playerFacade.GetPlayer(Context.ConnectionId);
        var room = _roomFacade.GetRoom(roomId);
        if (player is null || room is null)
        {
            response.Success = false;
            response.Description = "Player or room does not exist";
            return response;
        }
        
        // Validate the player requesting is in the Room
        if (!room.Players.Contains(player))
        {
            response.Success = false;
            response.Description = "Player was not found in the room";
            return response;
        }

        var game = new SonnetsForSimpletons(new QuestionFacade());
        room.Game = game;
        game.StartGame();
        
        response.Success = true;
        response.Description = "Game has been started";
        return response;
    }
}