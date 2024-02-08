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

    public void UpdateScore(string homeTeam, string awayTeam, int homeScore, int awayScore)
    {
        if (string.IsNullOrWhiteSpace(homeTeam) || string.IsNullOrWhiteSpace(awayTeam))
            throw new ArgumentException("Invalid input.");
        
        if (homeScore < 0 || awayScore < 0)
            throw new ArgumentException("Invalid input. Score cannot be a negative value.");
        
        var match = Matches.FirstOrDefault(m => m.HomeTeam == homeTeam && m.AwayTeam == awayTeam);

        if (match == null)
        {
            throw new InvalidOperationException("Match does not exist.");
        }

        match.HomeScore = homeScore;
        match.AwayScore = awayScore;
    }

    public void FinishMatch(string homeTeam, string awayTeam)
    {
        if (string.IsNullOrWhiteSpace(homeTeam) || string.IsNullOrWhiteSpace(awayTeam))
            throw new ArgumentException("Invalid input.");
        
        var match = Matches.FirstOrDefault(m => m.HomeTeam == homeTeam && m.AwayTeam == awayTeam);
        
        if (match == null)
        {
            throw new InvalidOperationException("Match does not exist.");
        }

        Matches.Remove(match);
    }
    
    public IList<FootballMatch> Matches { get; } = new List<FootballMatch>();

    public class FootballMatch(string homeTeam, string awayTeam)
    {
        public string HomeTeam { get; } = homeTeam;
        public string AwayTeam { get; } = awayTeam;
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
    }
    
    private bool IsTeamAlreadyPlaying(string team)
    {
        return Matches.Any(m => m.HomeTeam == team || m.AwayTeam == team);
    }
}