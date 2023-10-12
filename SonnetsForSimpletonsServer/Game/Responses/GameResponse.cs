using SonnetsForSimpletonsServer.Lobby.Responses;

namespace SonnetsForSimpletonsServer.Game.Responses;

public class GameResponse : GeneralResponse
{
    public string? TeamName { get; set; }
}