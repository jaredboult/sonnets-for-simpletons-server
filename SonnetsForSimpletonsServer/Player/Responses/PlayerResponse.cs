using SonnetsForSimpletonsServer.Lobby.Responses;

namespace SonnetsForSimpletonsServer.Player.Responses;

public class PlayerResponse : GeneralResponse
{
    public string Name { get; set; } = "Anonymous";
    public string? Id { get; set; }
}