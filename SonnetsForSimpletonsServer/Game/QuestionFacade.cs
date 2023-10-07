using SonnetsForSimpletonsServer.Game.Models;

namespace SonnetsForSimpletonsServer.Game;

public class QuestionFacade : IQuestionFacade
{
    private List<IQuestion> _questions =
    new() {
        new Question("Quiz", "Pop Quiz"),
        new Question("Side", "Bedside"),
        new Question("Love", "LoveLetter"),
        new Question("Mind", "Mind Reader"),
        new Question("Tongue", "Tongue-tied"),
        new Question("Skin", "Snake Skin")
    };

    private List<IQuestion> _completedQuestions = new();
    private readonly Random _randomNumberGenerator = new();

    public IQuestion GetQuestion()
    {
        if (_questions.Count == 0)
        {
            _questions = _completedQuestions;
            _completedQuestions = new List<IQuestion>();
        }
        var index = _randomNumberGenerator.Next(0, _questions.Count);
        var question = _questions[index];
        _questions.RemoveAt(index);
        _completedQuestions.Add(question);
        return question;
    }
}