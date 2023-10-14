using SonnetsForSimpletonsServer.Player.Models;

namespace SonnetsForSimpletonsServer.Game.Models;

public class Team : ITeam
{
    public int Id { get; init; }
    public IList<IPlayer> Players { get; init; }
    public int Score { get; set; }

    public Team(int id, IList<IPlayer> players)
    {
        Id = id;
        Players = players;
    }
}