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
    /// <returns>A new IRoom object if succesful</returns>
    public Room CreateRoom();
    
    /// <summary>
    /// Joins a room, if it exists
    /// </summary>
    /// <param name="roomCode">The four letter room code</param>
    /// <returns>True if the room exists and can be joined, false otherwise.</returns>
    public bool JoinRoom(string roomCode);
}