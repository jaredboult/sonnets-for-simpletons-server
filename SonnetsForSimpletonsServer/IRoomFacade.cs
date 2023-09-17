using SonnetsForSimpletonsServer.Models;
using SonnetsForSimpletonsServer.Models.Lobby;

namespace SonnetsForSimpletonsServer;

/// <summary>
/// A facade to abstract the logic for the creating, joining, and deleting rooms
/// </summary>
public interface IRoomFacade
{
    /// <summary>
    /// Creates a new room 
    /// </summary>
    /// <throws>An ApplicationException if the room limit is reached</throws>
    /// <returns>A new IRoom object if successful</returns>
    public Room CreateRoom(Player host);
    
    /// <summary>
    /// Joins a room, if it exists
    /// </summary>
    /// <param name="joiner">The Player who is attempting to join using a room code</param>
    /// <param name="roomCode">The unique four letter room code</param>
    /// <returns>True if the room exists and can be joined, false otherwise.</returns>
    public bool JoinRoom(Player joiner, string roomCode);

    /// <summary>
    /// Returns a room with the matching roomCode, if it exists.
    /// </summary>
    /// <param name="roomCode">The unique four digit room code</param>
    /// <returns>A room object if it exists, otherwise null</returns>
    public Room? GetRoom(string roomCode);
}