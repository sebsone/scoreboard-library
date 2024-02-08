namespace Scoreboard;

public interface IScoreboard
{
    /// <summary>
    /// Initializes and starts a new match with the given teams.
    /// </summary>
    /// <param name="homeTeam">The name of the home team.</param>
    /// <param name="awayTeam">The name of the away team.</param>
    /// <exception cref="ArgumentException">Thrown when either team name is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if a match with either team already exists.</exception>
    void StartMatch(string homeTeam, string awayTeam);
}