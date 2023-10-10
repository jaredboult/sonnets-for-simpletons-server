using SonnetsForSimpletonsServer.Player.Models;
using SonnetsForSimpletonsServer.Game.Models;

namespace SonnetsForSimpletonsServer.Game;

public interface IGame
{
    public string Name { get; }
    public GameProgress Progress { get; set; }
    public Dictionary<IPlayer, int> Scores { get; set; }
    public IQuestionFacade QuestionFacade { get; init; }
}