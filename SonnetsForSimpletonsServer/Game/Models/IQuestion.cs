namespace SonnetsForSimpletonsServer.Game.Models;

public interface IQuestion
{
    public IDictionary<int, string> Answers { get; }
}