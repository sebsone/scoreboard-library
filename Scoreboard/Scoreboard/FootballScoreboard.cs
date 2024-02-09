namespace Scoreboard;

public class FootballScoreboard : IScoreboard
{
    public void StartMatch(string homeTeam, string awayTeam)
    {
        if (string.IsNullOrWhiteSpace(homeTeam) || string.IsNullOrWhiteSpace(awayTeam))
            throw new ArgumentException("Invalid input.");
        
        if (IsTeamAlreadyPlaying(homeTeam) || IsTeamAlreadyPlaying(awayTeam))
            throw new InvalidOperationException("One or both teams does already have a match in progress.");
        
        Matches.Add((homeTeam, awayTeam), new FootballMatch(homeTeam, awayTeam));
    }

    public void UpdateScore(string homeTeam, string awayTeam, int homeScore, int awayScore)
    {
        if (string.IsNullOrWhiteSpace(homeTeam) || string.IsNullOrWhiteSpace(awayTeam))
            throw new ArgumentException("Invalid input.");
        
        if (IsScoreInvalid(homeScore) || IsScoreInvalid(awayScore))
            throw new ArgumentException("Invalid input. Score cannot be a negative value.");

        if (!Matches.TryGetValue((homeTeam, awayTeam), out var match))
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

        if (!Matches.Remove((homeTeam, awayTeam))) throw new InvalidOperationException("Match does not exist.");
    }

    public List<IScoreboardMatch> GetScoreboardSummary() => (
        new (
            Matches.Values
                .OrderByDescending(m => m.HomeScore + m.AwayScore)
                .ThenByDescending(m => m.StartDate)
                .ToList())
    );

    public IDictionary<(string HomeTeam, string AwayTeam), FootballMatch> Matches { get; } = new Dictionary<(string, string), FootballMatch>();

    public class FootballMatch(string homeTeam, string awayTeam) : IScoreboardMatch
    {
        public string HomeTeam { get; } = homeTeam;
        public string AwayTeam { get; } = awayTeam;
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public DateTime StartDate { get; } = DateTime.UtcNow;
    }
    
    private bool IsTeamAlreadyPlaying(string team)
    {
        return Matches.Any(m => m.Key.HomeTeam == team || m.Key.AwayTeam == team);
    }

    private static bool IsScoreInvalid(int score)
    {
        return score < 0;
    }
}