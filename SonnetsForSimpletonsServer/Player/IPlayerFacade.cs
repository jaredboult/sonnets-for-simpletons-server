using SonnetsForSimpletonsServer.Player.Models;

namespace SonnetsForSimpletonsServer.Player;

public interface IPlayerFacade
{
    public IPlayer CreatePlayer(string connectionId);

    /// <summary>
    /// Performs some validation before updating a Player's name.
    /// </summary>
    /// <param name="name">The name to check before updating</param>
    /// <param name="connectionId">The connection string of the player</param>
    /// <returns>Returns the trimmed and validated name if successful, null otherwise</returns>
    public string? UpdatePlayerName(string name, string connectionId);

    /// <summary>
    /// Retrieves the Player object that corresponds to a given connectionId
    /// </summary>
    /// <param name="connectionId"></param>
    /// <returns>The Player object if a match is found, otherwise null</returns>
    public IPlayer? GetPlayer(string connectionId);
}