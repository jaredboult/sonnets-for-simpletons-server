using SonnetsForSimpletonsServer.Player.Models;

namespace SonnetsForSimpletonsServer.Lobby.Models;

public interface IRoom
{
    string RoomCode { get; init; }
    IList<IPlayer> Players { get; }
    void AddPlayer(IPlayer player);
}