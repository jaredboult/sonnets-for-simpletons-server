using SonnetsForSimpletonsServer.Game.Responses;

namespace SonnetsForSimpletonsServer.Game;

public interface IGameClient
{
    public Task UpdateQuestion(QuestionResponse response);
    public Task NotifyTeam(GameResponse response);
}