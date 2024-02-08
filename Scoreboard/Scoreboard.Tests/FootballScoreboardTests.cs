using FluentAssertions;

namespace Scoreboard.Tests;

public class FootballScoreboardTests
{
    [Theory]
    [InlineData("Norway", "Sweden")]
    [InlineData("South Korea", "Japan")]
    public void StartMatch_WithTwoNewTeams_MatchIsCreated(string homeTeam, string awayTeam)
    {
        // Assemble
        var scoreboard = new FootballScoreboard();

        // Act
        scoreboard.StartMatch(homeTeam, awayTeam);

        // Assert
        var match = scoreboard.Matches.FirstOrDefault();
        match.Should().NotBeNull("because a new match should have been started.");
        match.HomeTeam.Should().Be(homeTeam, $"because the home team should be set to {homeTeam}.");
        match.AwayTeam.Should().Be(awayTeam, $"because the away team should be set to {awayTeam}.");
    }
    
    [Fact]
    public void StartMatch_WithMultipleNewTeams_MatchesAreCreated()
    {
        var scoreBoard = new FootballScoreboard();
    
        scoreBoard.StartMatch("Norway", "Sweden");
        scoreBoard.StartMatch("Mexico", "USA");
        scoreBoard.StartMatch("England", "Scotland");
    
        scoreBoard.Matches.Should().HaveCount(3, "because three matches were started");

        var expectedMatches = new List<(string homeTeam, string awayTeam)>
        {
            ("Norway", "Sweden"),
            ("Mexico", "USA"),
            ("England", "Scotland")
        };

        scoreBoard.Matches.Select(m => (m.HomeTeam, m.AwayTeam))
            .Should().Equal(expectedMatches, "because the matches should be added in the order they were started");
    }
    
    [Fact]
    public void StartMatch_WithEmptyTeamNames_ThrowsException()
    {
        var scoreBoard = new FootballScoreboard();
        
        Action act = () => scoreBoard.StartMatch("", "");

        act.Should().Throw<ArgumentException>().WithMessage("Invalid input.");
    }
    
    [Fact]
    public void StartMatch_WithWhitespaceTeamNames_ThrowsException()
    {
        var scoreBoard = new FootballScoreboard();
        
        Action act = () => scoreBoard.StartMatch(" ", " ");

        act.Should().Throw<ArgumentException>().WithMessage("Invalid input.");
    }
    
    [Fact]
    public void StartMatch_WithBothTeamsAlreadyPlaying_ThrowsException()
    {
        var scoreBoard = new FootballScoreboard();
        
        scoreBoard.StartMatch("Norway", "Sweden");
        Action act = () => scoreBoard.StartMatch("Norway", "Sweden");

        act.Should().Throw<InvalidOperationException>().WithMessage("One or both teams does already have a match in progress.");
    }
    
    [Fact]
    public void StartMatch_WithOneTeamAlreadyPlaying_ThrowsException()
    {
        var scoreBoard = new FootballScoreboard();
        
        scoreBoard.StartMatch("Norway", "Sweden");
        var act = () => scoreBoard.StartMatch("Norway", "Portugal");

        act.Should().Throw<InvalidOperationException>().WithMessage("One or both teams does already have a match in progress.");
    }
}