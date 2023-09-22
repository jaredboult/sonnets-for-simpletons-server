namespace SonnetsForSimpletonsServer.Game;

public class QuestionFacade
{
    private List<(string, string)> _questions =
    new() {
        ("Quiz", "Pop Quiz"),
        ("Side", "Bedside"),
        ("Love", "LoveLetter"),
        ("Mind", "Mind Reader"),
        ("Tongue", "Tongue-tied"),
        ("Skin", "Snake Skin")
    };

    private List<(string, string)> _completedQuestions = new();
    private readonly Random _randomNumberGenerator = new();

    public (string, string) GetQuestion()
    {
        if (_questions.Count == 0)
        {
            _questions = _completedQuestions;
            _completedQuestions = new List<(string, string)>();
        }
        var index = _randomNumberGenerator.Next(0, _questions.Count);
        var question = _questions[index];
        _questions.RemoveAt(index);
        _completedQuestions.Add(question);
        return question;
    }
}