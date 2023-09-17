using System.Collections.Concurrent;
using SonnetsForSimpletonsServer.Models;

namespace SonnetsForSimpletonsServer;

public class PlayerFacade : IPlayerFacade
{
    private readonly ConcurrentDictionary<string, Player> _players = new ();

    public Player CreatePlayer(string connectionId)
    {
        var player = new Player(connectionId);
        _players.TryAdd(connectionId, player);
        return player;
    }
    
    public bool UpdatePlayerName(string name, string connectionId)
    {
        if (!_players.TryGetValue(connectionId, out var player))
        {
            throw new ApplicationException("Player with matching connection id does not exist");
        }
        var namePassedValidation = ValidatePlayerName(name);
        if (namePassedValidation)
        {
            player.Name = name;
        }
        return namePassedValidation;
    }

    private static bool ValidatePlayerName(string name)
    {
        return name.Length is > 1 and < 30;
    }
}