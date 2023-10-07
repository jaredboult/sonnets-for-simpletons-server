using SonnetsForSimpletonsServer.Lobby.Responses;

namespace SonnetsForSimpletonsServer.Game.Responses;

public class QuestionResponse : GeneralResponse
{
    public IDictionary<int, string> Answers = new Dictionary<int, string>();
}