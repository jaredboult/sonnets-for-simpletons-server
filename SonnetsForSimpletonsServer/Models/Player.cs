namespace SonnetsForSimpletonsServer.Models;

public class Player
{
    public string Name { get; set; } = "Anonymous";
    public string ConnectionId { get; set; }

    public Player(string connectionId)
    {
        ConnectionId = connectionId;
    }
}