using SonnetsForSimpletonsServer.Player.Models;

namespace SonnetsForSimpletonsServer.Game.Models;

public interface ITeam
{
    public int Id { get; init; }
    public IList<IPlayer> Players { get; init; }
    public int Score { get; set; }
}