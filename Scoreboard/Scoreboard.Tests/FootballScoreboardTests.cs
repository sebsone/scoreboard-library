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
    
    [Theory]
    [InlineData("Argentina", "France", 3, 3)]
    [InlineData("Germany", "Brazil", 7, 1)]
    public void UpdateScore_WithValidScores_UpdatesMatchScore(string homeTeam, string awayTeam, int homeScore, int awayScore)
    {
        var scoreBoard = new FootballScoreboard();
        
        scoreBoard.StartMatch(homeTeam, awayTeam);
        scoreBoard.UpdateScore(homeTeam, awayTeam, homeScore, awayScore);
        var match = scoreBoard.Matches.FirstOrDefault(match =>
            match.HomeTeam == homeTeam && match.AwayTeam == awayTeam);

        match.Should().NotBeNull("because updating the score should remove the match object.");
        match.HomeScore.Should().Be(homeScore, $"because the new home score should be set to {homeScore}");
        match.AwayScore.Should().Be(awayScore, $"because the new home score should be set to {awayScore}");
    }
    
    [Fact]
    public void UpdateScore_WithValidScoresAndMultipleMatchesOngoing_UpdatesCorrectMatchScoreOnly()
    {
        var scoreBoard = new FootballScoreboard();
        scoreBoard.StartMatch("Spain", "Portugal");
        scoreBoard.StartMatch("Austria", "Switzerland");
        scoreBoard.StartMatch("Bulgaria", "Romania");
        
        scoreBoard.UpdateScore("Austria", "Switzerland", 1, 3);
        var updatedMatch = scoreBoard.Matches.FirstOrDefault(match =>
            match is { HomeTeam: "Austria", AwayTeam: "Switzerland" });
        
        
        updatedMatch.Should().NotBeNull();
        updatedMatch.HomeScore.Should().Be(1, "because this score was updated.");
        updatedMatch.AwayScore.Should().Be(3, "because this score was updated.");
        scoreBoard.Matches.Where(otherMatch => otherMatch is not { HomeTeam: "Austria", AwayTeam: "Switzerland" }).ToList()
            .ForEach(
                otherMatch =>
                {
                    otherMatch.HomeScore.Should().Be(0, "because the other matches should not be affected.");
                    otherMatch.AwayScore.Should().Be(0, "because the other matches should not be affected.");
                });
    }

    [Fact]
    public void UpdateScore_WithNonExistentMatch_ThrowsException()
    {
        var scoreBoard = new FootballScoreboard();
        
        var act = () => scoreBoard.UpdateScore("Peru", "Colombia", 2, 3);

        act.Should().Throw<InvalidOperationException>().WithMessage("Match does not exist.");
    }
    
    [Fact]
    public void UpdateScore_WithEmptyTeams_ThrowsException()
    {
        var scoreBoard = new FootballScoreboard();
        
        var act = () => scoreBoard.UpdateScore("", "", 2, 3);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid input.");
    }
    
    [Fact]
    public void UpdateScore_WithWhitespaceTeams_ThrowsException()
    {
        var scoreBoard = new FootballScoreboard();
        
        var act = () => scoreBoard.UpdateScore("     ", " ", 2, 3);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid input.");
    }
    
    [Fact]
    public void UpdateScore_WithNegativeScores_ThrowsException()
    {
        var scoreBoard = new FootballScoreboard();
        
        var act = () => scoreBoard.UpdateScore("Nigeria", "Ivory Coast", -2, -1);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid input. Score cannot be a negative value.");
    }
}