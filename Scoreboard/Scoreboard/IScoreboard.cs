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

    /// <summary>
    /// Updates the scores of a given match on the current scoreboard.
    /// </summary>
    /// <param name="homeTeam">The name of the home team.</param>
    /// <param name="awayTeam">The name of the away team.</param>
    /// <param name="homeScore">The score of the home team.</param>
    /// <param name="awayScore">The score of the away team.</param>
    /// <exception cref="ArgumentException">Thrown when either team name is null or empty, or when either score is a negative value.</exception>
    /// <exception cref="InvalidOperationException">Thrown if a match does not currently exist.</exception>
    void UpdateScore(string homeTeam, string awayTeam, int homeScore, int awayScore);
}