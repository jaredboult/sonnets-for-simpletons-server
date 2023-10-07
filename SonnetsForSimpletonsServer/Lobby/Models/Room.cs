using SonnetsForSimpletonsServer.Game;
using SonnetsForSimpletonsServer.Player.Models;

namespace SonnetsForSimpletonsServer.Lobby.Models;

public class Room : IRoom
{
    public string RoomCode { get; init; } = "";
    public IList<IPlayer> Players { get; } = new List<IPlayer>();
    public IGame? Game { get; set; }
    public void AddPlayer(IPlayer player)
    {
        Players.Add(player);
    }
}