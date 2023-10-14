using SonnetsForSimpletonsServer.Game.Models;

namespace SonnetsForSimpletonsServer.Game;

public interface IGame
{
    string Name { get; }
    byte NumberOfRounds { get; }
    GameProgress Progress { get; set; }
    IQuestionFacade QuestionFacade { get; init; }
    void StartGame();
    void EndGame();
}