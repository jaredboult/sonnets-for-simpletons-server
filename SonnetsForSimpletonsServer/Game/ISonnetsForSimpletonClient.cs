using SonnetsForSimpletonsServer.Game.Responses;

namespace SonnetsForSimpletonsServer.Game;

public interface ISonnetsForSimpletonClient
{
    public Task UpdateQuestion(QuestionResponse response);
}