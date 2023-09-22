namespace SonnetsForSimpletonsServer.Lobby.Responses;

public class RoomResponse : GeneralResponse
{
    public string? RoomId { get; set; }
    public IList<string> PlayerNames { get; set; } = new List<string>();
}