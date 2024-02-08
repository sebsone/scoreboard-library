namespace Scoreboard;

public class FootballScoreboard : IScoreboard
{
    public void StartMatch(string homeTeam, string awayTeam)
    {
        Match = new FootballMatch(homeTeam, awayTeam);
    }

    public FootballMatch Match = new("", "");

    public record FootballMatch(string HomeTeam, string AwayTeam);
}