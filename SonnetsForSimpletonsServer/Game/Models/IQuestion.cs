namespace SonnetsForSimpletonsServer.Game.Models;

public interface IQuestion
{
    /// <summary>
    /// Returns the answer with the matching number of points
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    string? GetAnswer(int key);
}