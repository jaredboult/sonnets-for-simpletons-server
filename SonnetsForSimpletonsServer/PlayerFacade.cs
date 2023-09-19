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

    public Player? GetPlayer(string connectionId)
    {
        _players.TryGetValue(connectionId, out var player);
        return player;
    }
    
    public string? UpdatePlayerName(string name, string connectionId)
    {
        var player = GetPlayer(connectionId);
        if (player is null)
        {
            throw new ApplicationException("Player with matching connection id does not exist");
        }
        var trimmedName = name.Trim();
        var namePassedValidation = ValidatePlayerName(trimmedName);
        if (!namePassedValidation)
        {
            return null;
        }
        player.Name = trimmedName;
        return trimmedName;
    }

    private static bool ValidatePlayerName(string name)
    {
        return name.Length is > 0 and < 30;
    }
}