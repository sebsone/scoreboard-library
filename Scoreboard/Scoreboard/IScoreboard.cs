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
    
    /// <summary>
    /// Finishes and removes the given match on the current scoreboard.
    /// </summary>
    /// <param name="homeTeam">The name of the home team.</param>
    /// <param name="awayTeam">The name of the away team.</param>
    /// <exception cref="ArgumentException">Thrown when either team name is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if a match does not currently exist.</exception>
    void FinishMatch(string homeTeam, string awayTeam);

    /// <summary>
    /// Returns a summary of matching currently in progress.
    /// </summary>
    /// <remarks>
    /// The summary is ordered primarily by the total score in descending order,
    /// and then by the start date of the match, with the most recently started match first.
    /// This method returns an empty list if there are no ongoing matches.
    /// </remarks>
    /// <returns>
    /// A list of type <see cref="IScoreboardMatch"/> representing the summary of ongoing matches.
    /// </returns>
    List<IScoreboardMatch> GetScoreboardSummary();
}

public interface IScoreboardMatch 
{
    string HomeTeam { get; }
    string AwayTeam { get; }
    int HomeScore { get; }
    int AwayScore { get; }
}