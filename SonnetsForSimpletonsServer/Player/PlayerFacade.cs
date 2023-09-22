using System.Collections.Concurrent;
using SonnetsForSimpletonsServer.Player.Models;

namespace SonnetsForSimpletonsServer.Player;

public class PlayerFacade : IPlayerFacade
{
    private readonly ConcurrentDictionary<string, IPlayer> _players = new();

    public IPlayer CreatePlayer(string connectionId)
    {
        var player = new Models.Player {
            ConnectionId = connectionId
        };
        if (!_players.TryAdd(connectionId, player))
        {
            _players.TryGetValue(connectionId, out var existingPlayer);
            _players.TryUpdate(connectionId, player, existingPlayer ?? new Models.Player());
        }
        return player;
    }

    public IPlayer? GetPlayer(string connectionId)
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