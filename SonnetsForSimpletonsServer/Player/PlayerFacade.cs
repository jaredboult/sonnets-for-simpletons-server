using System.Collections.Concurrent;
using SonnetsForSimpletonsServer.Player.Models;

namespace SonnetsForSimpletonsServer.Player;

public class PlayerFacade : IPlayerFacade
{
    private readonly ConcurrentDictionary<Guid, IPlayer> _players = new();

    public IPlayer CreatePlayer(string connectionId)
    {
        var id = Guid.NewGuid();
        var player = new Models.Player {
            ConnectionId = connectionId,
            Id = id
        };
        if (!_players.TryAdd(id, player))
        {
            _players.TryGetValue(id, out var existingPlayer);
            _players.TryUpdate(id, player, existingPlayer ?? new Models.Player());
        }
        return player;
    }

    public IPlayer? GetPlayer(string guid)
    {
        if (!Guid.TryParse(guid, out var id))
        {
            return null;
        }
        _players.TryGetValue(id, out var player);
        return player;
    }

    public string? UpdatePlayerName(string name, string id)
    {
        var player = GetPlayer(id);
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