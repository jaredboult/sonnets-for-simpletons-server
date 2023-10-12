using SonnetsForSimpletonsServer.Game.Models;
using SonnetsForSimpletonsServer.Player.Models;

namespace SonnetsForSimpletonsServer.Game;

public class FourOrMorePlayersGame : IGame
{
    public IList<IPlayer> TeamOne { get; private set; } = new List<IPlayer>();
    public IList<IPlayer> TeamTwo { get; private set; } = new List<IPlayer>();
    
    public string Name => "Sonnets for Simpletons";
    public GameProgress Progress { get; set; } = GameProgress.Initialising;
    public Dictionary<IPlayer, int> Scores { get; set; } = new ();
    public IQuestionFacade QuestionFacade { get; init; }
    

    public FourOrMorePlayersGame(IQuestionFacade questionFacade)
    {
        QuestionFacade = questionFacade;
    }

    public void SetRandomTeams(IEnumerable<IPlayer> players)
    {
        var shuffled = ShuffleList(players);
        var teams = SplitPlayersIntoTeams(shuffled, 2);
        TeamOne = teams[0];
        TeamTwo = teams[1];
    }

    public void StartGame()
    {
        Progress = GameProgress.InProgress;
    }
    
    public void EndGame()
    {
        Progress = GameProgress.Finished;
    }
    
    // Randomises the order of the list
    private static IList<T> ShuffleList<T> (IEnumerable<T> list)
    {
        var random = new Random();
        var clonedList = list.ToList();
        
        /*
         * Using Fisher-Yates Shuffle algorithm
         */
        for (var i = clonedList.Count - 1; i > 0; i--)
        {
            var k = random.Next(i + 1);
            (clonedList[k], clonedList[i]) = (clonedList[i], clonedList[k]);
        }
        return clonedList;
    }

    private static IList<IPlayer[]> SplitPlayersIntoTeams(ICollection<IPlayer> players, int numberOfTeams)
    {
        if (numberOfTeams < 1 || numberOfTeams > players.Count)
        {
            throw new ArgumentException("Invalid number of teams provided");
        }
        var teamSize = Convert.ToInt32(Math.Ceiling(players.Count / Convert.ToDouble(numberOfTeams)));
        return players.Chunk(teamSize).ToList();
    }
}