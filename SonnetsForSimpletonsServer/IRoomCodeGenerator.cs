namespace SonnetsForSimpletonsServer;

/// <summary>
/// Generates the unique room codes for each room
/// </summary>
public interface IRoomCodeGenerator
{
    /// <summary>
    /// Generates a four letter room code
    /// </summary>
    /// <returns>A new and unique room code</returns>
    public string GenerateRoomCode();
}