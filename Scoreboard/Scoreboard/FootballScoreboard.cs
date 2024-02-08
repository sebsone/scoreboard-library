namespace Scoreboard;

public class FootballScoreboard : IScoreboard
{
    public void StartMatch(string homeTeam, string awayTeam)
    {
        if (string.IsNullOrWhiteSpace(homeTeam) || string.IsNullOrWhiteSpace(awayTeam))
            throw new ArgumentException("Invalid input.");
        
        if (IsTeamAlreadyPlaying(homeTeam) || IsTeamAlreadyPlaying(awayTeam))
            throw new InvalidOperationException("One or both teams does already have a match in progress.");
        
        Matches.Add(new FootballMatch(homeTeam, awayTeam));
    }

    public IList<FootballMatch> Matches { get; } = new List<FootballMatch>();

    public record FootballMatch(string HomeTeam, string AwayTeam);
    
    private bool IsTeamAlreadyPlaying(string team)
    {
        return Matches.Any(m => m.HomeTeam == team || m.AwayTeam == team);
    }
}