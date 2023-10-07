using SonnetsForSimpletonsServer.Game;
using SonnetsForSimpletonsServer.Player.Models;

namespace SonnetsForSimpletonsServer.Lobby.Models;

public interface IRoom
{
    string RoomCode { get; init; }
    IList<IPlayer> Players { get; }
    IGame? Game { get; set; }
    void AddPlayer(IPlayer player);
}