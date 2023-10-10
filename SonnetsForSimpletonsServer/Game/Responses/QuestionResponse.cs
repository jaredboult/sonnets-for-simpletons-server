using SonnetsForSimpletonsServer.Lobby.Responses;

namespace SonnetsForSimpletonsServer.Game.Responses;

public class QuestionResponse : GeneralResponse
{
    public IDictionary<int, string> Answers { get; set; } = new Dictionary<int, string>();
}