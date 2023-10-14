using SonnetsForSimpletonsServer.Game.Models;
using SonnetsForSimpletonsServer.Player.Models;

namespace SonnetsForSimpletonsServer.Game;

public class FourPlusPlayerGame : IGame
{
    public string Name => "Sonnets for Simpletons";
    public byte NumberOfRounds => 2;
    public byte NumberOfTeams => 2;
    public GameProgress Progress { get; set; } = GameProgress.Initialising;
    public IQuestionFacade QuestionFacade { get; init; }
    public IList<ITeam> Teams { get; private set; } = new List<ITeam>();
    
    

    public FourPlusPlayerGame(IQuestionFacade questionFacade)
    {
        QuestionFacade = questionFacade;
    }

    public void SetRandomTeams(IEnumerable<IPlayer> players)
    {
        var shuffled = ShuffleList(players);
        var teams = SplitPlayersIntoTeams(shuffled, NumberOfTeams);
        for (var i = 0; i < NumberOfTeams; i++)
        {
            Teams.Add(teams[i]);
        }
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

    private static IList<ITeam> SplitPlayersIntoTeams(ICollection<IPlayer> players, int numberOfTeams)
    {
        if (numberOfTeams < 1 || numberOfTeams > players.Count)
        {
            throw new ArgumentException("Invalid number of teams provided");
        }
        var teamSize = Convert.ToInt32(Math.Ceiling(players.Count / Convert.ToDouble(numberOfTeams)));
        var playersList = players.Chunk(teamSize).ToList();
        
        var result = new List<ITeam>();
        for (var i = 0; i < numberOfTeams; i++)
        {
            result.Add(new Team(i+1, new List<IPlayer>(playersList[i])));
        }
        return result;
    }
}