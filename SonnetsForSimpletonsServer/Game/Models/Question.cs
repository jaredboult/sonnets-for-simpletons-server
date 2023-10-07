namespace SonnetsForSimpletonsServer.Game.Models;

public class Question : IQuestion
{
    private IDictionary<int, string> Answers { get; set; } = new Dictionary<int, string>();
    
    public Question(string firstPart, string secondPart)
    {
        Answers.Add(1, firstPart);
        Answers.Add(3, secondPart);
    }

    /// <inheritdoc/>
    public string? GetAnswer(int key)
    {
        Answers.TryGetValue(key, out var result);
        return result;
    }
}