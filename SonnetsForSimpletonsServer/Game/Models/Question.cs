namespace SonnetsForSimpletonsServer.Game.Models;

public class Question : IQuestion
{
    public IDictionary<int, string> Answers { get; } = new Dictionary<int, string>();
    
    public Question(string firstPart, string secondPart)
    {
        Answers.Add(1, firstPart);
        Answers.Add(3, secondPart);
    }
}