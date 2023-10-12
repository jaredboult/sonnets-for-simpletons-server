using Microsoft.AspNetCore.SignalR;
using SonnetsForSimpletonsServer.Game.Models;
using SonnetsForSimpletonsServer.Game.Responses;
using SonnetsForSimpletonsServer.Lobby;
using SonnetsForSimpletonsServer.Lobby.Models;
using SonnetsForSimpletonsServer.Player;
using SonnetsForSimpletonsServer.Player.Models;

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

    public async Task<GameResponse> StartGame(string roomId, string playerGuid)
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
        await SetUpTeams(game, room);
        
        game.StartGame();
        
        response.Success = true;
        response.Description = "Game has been started";
        return response;
    }

    private async Task SetUpTeams(FourOrMorePlayersGame game, IRoom room)
    {
        game.SetRandomTeams(room.Players);
        var firstGroupName = $"{room.RoomCode}_TeamOne";
        var secondGroupName = $"{room.RoomCode}_TeamTwo";
        await AddPlayersToGroup(game.TeamOne, firstGroupName, "Team One");
        await AddPlayersToGroup(game.TeamTwo, secondGroupName, "Team Two");
        
    }

    private async Task AddPlayersToGroup(IEnumerable<IPlayer> players, string groupNameForHub, string teamNameForClient)
    {
        foreach (var player in players)
        {
            if (player.ConnectionId is not null)
            {
                await Groups.AddToGroupAsync(player.ConnectionId, groupNameForHub);
            }
        }
        await Clients.Group(groupNameForHub).NotifyTeam(new GameResponse
        {
            Success = true,
            TeamName = teamNameForClient
        });
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