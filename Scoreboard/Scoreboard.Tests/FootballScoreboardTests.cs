using FluentAssertions;

namespace Scoreboard.Tests;

public class FootballScoreboardTests
{
    [Fact]
    public void StartMatch_WithTwoNewTeams_MatchIsCreated()
    {
        // Assemble
        var scoreboard = new FootballScoreboard();

        // Act
        scoreboard.StartMatch("Norway", "Sweden");

        // Assert
        scoreboard.Match.Should().NotBeNull("because a new match should have been started.");
        scoreboard.Match.HomeTeam.Should().Be("Norway", "because the home team should be set to Norway.");
        scoreboard.Match.AwayTeam.Should().Be("Sweden", "because the away team should be set to Sweden.");
    }
}