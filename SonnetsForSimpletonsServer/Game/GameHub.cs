using Microsoft.AspNetCore.SignalR;
using SonnetsForSimpletonsServer.Game.Models;
using SonnetsForSimpletonsServer.Game.Responses;
using SonnetsForSimpletonsServer.Lobby;
using SonnetsForSimpletonsServer.Player;

namespace SonnetsForSimpletonsServer.Game;

public class GameHub : Hub<IGameClient>
{
    private readonly IRoomFacade _roomFacade;
    private readonly IPlayerFacade _playerFacade;

    public GameHub(IRoomFacade roomFacade, IPlayerFacade playerFacade)
    {
        _roomFacade = roomFacade;
        _playerFacade = playerFacade;
    }

    public GameResponse StartGame(string roomId, string playerGuid)
    {
        var response = new GameResponse();
        var player = _playerFacade.GetPlayer(playerGuid);
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

        var game = new FourOrMorePlayersGame(new QuestionFacade());
        room.Game = game;
        game.StartGame();
        
        response.Success = true;
        response.Description = "Game has been started";
        return response;
    }

    public QuestionResponse GetQuestion(string roomId)
    {
        var response = new QuestionResponse();
        var room = _roomFacade.GetRoom(roomId);

        if (room is null)
        {
            response.Success = false;
            response.Description = "Room does not exist";
            return response;
        }

        var game = room.Game;
        if (game is null)
        {
            response.Success = false;
            response.Description = "Game does not exist";
            return response;
        }

        if (game.Progress is not GameProgress.InProgress)
        {
            response.Success = false;
            response.Description = "Game state is not In Progress";
            return response;
        }

        var question = game.QuestionFacade.GetQuestion();
        response.Answers = question.Answers;
        response.Success = true;
        Clients.Others.UpdateQuestion(response);
        return response;
    }
    
}