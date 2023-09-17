namespace SonnetsForSimpletonsServer.Models.Lobby;

public class Room
{
    public string RoomCode { get; init; } = "";
    public IList<Player> Players { get; } = new List<Player>();

    public void AddPlayer(Player player)
    {
        Players.Add(player);
    }
}