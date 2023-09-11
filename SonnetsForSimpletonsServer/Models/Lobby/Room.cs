namespace SonnetsForSimpletonsServer.Models.Lobby;

public class Room
{
    public string RoomCode { get; init; } = "";
    private List<Player> Players { get; } = new ();

    public void AddPlayer(Player player)
    {
        Players.Add(player);
    }
}