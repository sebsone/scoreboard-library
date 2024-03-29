using FluentAssertions;

namespace Scoreboard.Tests;

public class FootballScoreboardTests
{
    [Theory]
    [InlineData("Norway", "Sweden")]
    [InlineData("South Korea", "Japan")]
    public void StartMatch_WithTwoNewTeams_CreatesMatch(string homeTeam, string awayTeam)
    {
        // Assemble
        var scoreboard = new FootballScoreboard();

        // Act
        scoreboard.StartMatch(homeTeam, awayTeam);

        // Assert
        var match = scoreboard.Matches[(homeTeam, awayTeam)];
        match.Should().NotBeNull("because a new match should have been started.");
        match.HomeTeam.Should().Be(homeTeam, $"because the home team should be set to {homeTeam}.");
        match.AwayTeam.Should().Be(awayTeam, $"because the away team should be set to {awayTeam}.");
    }
    
    [Fact]
    public void StartMatch_WithMultipleNewTeams_CreatesMatches()
    {
        var scoreboard = new FootballScoreboard();
    
        scoreboard.StartMatch("Norway", "Sweden");
        scoreboard.StartMatch("Mexico", "USA");
        scoreboard.StartMatch("England", "Scotland");
    
        var expectedMatches = new List<(string homeTeam, string awayTeam)>
        {
            ("Norway", "Sweden"),
            ("Mexico", "USA"),
            ("England", "Scotland")
        };
        scoreboard.Matches.Should().HaveCount(3, "because three matches were started");
        scoreboard.Matches.Keys.Should().BeEquivalentTo(expectedMatches, "because all started matches should be on the scoreboard.");
    }
    
    [Fact]
    public void StartMatch_WithEmptyTeamNames_ThrowsException()
    {
        var scoreboard = new FootballScoreboard();
        
        Action act = () => scoreboard.StartMatch("", "");

        act.Should().Throw<ArgumentException>().WithMessage("Invalid input.");
    }
    
    [Fact]
    public void StartMatch_WithWhitespaceTeamNames_ThrowsException()
    {
        var scoreboard = new FootballScoreboard();
        
        Action act = () => scoreboard.StartMatch(" ", " ");

        act.Should().Throw<ArgumentException>().WithMessage("Invalid input.");
    }
    
    [Theory]
    [InlineData("", "Norway")]
    [InlineData("Norway", "")]
    [InlineData(" ", "Norway")]
    [InlineData("Norway", "  ")]
    public void StartMatch_WithOneEmptyTeamName_ThrowsException(string homeTeam, string awayTeam)
    {
        var scoreboard = new FootballScoreboard();
        
        Action act = () => scoreboard.StartMatch(homeTeam, awayTeam);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid input.");
    }
    
    [Fact]
    public void StartMatch_WithBothTeamsAlreadyPlaying_ThrowsException()
    {
        var scoreboard = new FootballScoreboard();
        
        scoreboard.StartMatch("Norway", "Sweden");
        Action act = () => scoreboard.StartMatch("Norway", "Sweden");

        act.Should().Throw<InvalidOperationException>().WithMessage("One or both teams does already have a match in progress.");
    }
    
    [Fact]
    public void StartMatch_WithOneTeamAlreadyPlaying_ThrowsException()
    {
        var scoreboard = new FootballScoreboard();
        
        scoreboard.StartMatch("Norway", "Sweden");
        var act = () => scoreboard.StartMatch("Norway", "Portugal");

        act.Should().Throw<InvalidOperationException>().WithMessage("One or both teams does already have a match in progress.");
    }
    
    [Theory]
    [InlineData("Argentina", "France", 3, 3)]
    [InlineData("Germany", "Brazil", 7, 1)]
    public void UpdateScore_WithValidScores_UpdatesMatchScore(string homeTeam, string awayTeam, int homeScore, int awayScore)
    {
        var scoreboard = new FootballScoreboard();
        scoreboard.StartMatch(homeTeam, awayTeam);
        
        scoreboard.UpdateScore(homeTeam, awayTeam, homeScore, awayScore);
        
        var match = scoreboard.Matches[(homeTeam, awayTeam)];
        match.Should().NotBeNull("because updating the score should remove the match object.");
        match.HomeScore.Should().Be(homeScore, $"because the new home score should be set to {homeScore}");
        match.AwayScore.Should().Be(awayScore, $"because the new home score should be set to {awayScore}");
    }
    
    [Fact]
    public void UpdateScore_WithValidScoresAndMultipleMatchesOngoing_UpdatesCorrectMatchScoreOnly()
    {
        var scoreboard = new FootballScoreboard();
        scoreboard.StartMatch("Spain", "Portugal");
        scoreboard.StartMatch("Austria", "Switzerland");
        scoreboard.StartMatch("Bulgaria", "Romania");
        
        scoreboard.UpdateScore("Austria", "Switzerland", 1, 3);

        // Assert that the updated match is updated
        var updatedMatch = scoreboard.Matches[("Austria", "Switzerland")];
        updatedMatch.Should().NotBeNull();
        updatedMatch.HomeScore.Should().Be(1, "because this score was updated.");
        updatedMatch.AwayScore.Should().Be(3, "because this score was updated.");
        
        // Assert that the other matched remain unchanged
        foreach (var otherMatch in scoreboard.Matches)
        {
            if (otherMatch.Key != ("Austria", "Switzerland"))
            {
                otherMatch.Value.HomeScore.Should().Be(0, "because the other matches should not be affected.");
                otherMatch.Value.AwayScore.Should().Be(0, "because the other matches should not be affected.");
            }
        }
    }

    [Fact]
    public void UpdateScore_WithNonExistentMatch_ThrowsException()
    {
        var scoreboard = new FootballScoreboard();
        
        var act = () => scoreboard.UpdateScore("Peru", "Colombia", 2, 3);

        act.Should().Throw<InvalidOperationException>().WithMessage("Match does not exist.");
    }
    
    [Fact]
    public void UpdateScore_WithEmptyTeams_ThrowsException()
    {
        var scoreboard = new FootballScoreboard();
        
        var act = () => scoreboard.UpdateScore("", "", 2, 3);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid input.");
    }
    
    [Fact]
    public void UpdateScore_WithOneEmptyTeams_ThrowsException()
    {
        var scoreboard = new FootballScoreboard();
        
        var act = () => scoreboard.UpdateScore("Morocco", "", 2, 3);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid input.");
    }
    
    [Fact]
    public void UpdateScore_WithWhitespaceTeams_ThrowsException()
    {
        var scoreboard = new FootballScoreboard();
        
        var act = () => scoreboard.UpdateScore("     ", " ", 2, 3);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid input.");
    }
    
    [Fact]
    public void UpdateScore_WithNegativeScores_ThrowsException()
    {
        var scoreboard = new FootballScoreboard();
        
        var act = () => scoreboard.UpdateScore("Nigeria", "Ivory Coast", -2, -1);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid input. Score cannot be a negative value.");
    }
    
    [Theory]
    [InlineData("Norway", "Brazil")]
    [InlineData("Estonia", "Latvia")]
    public void FinishMatch_WithValidMatch_RemovesMatchFromScoreboard(string homeTeam, string awayTeam)
    {
        var scoreboard = new FootballScoreboard();
        scoreboard.StartMatch(homeTeam, awayTeam);
        
        scoreboard.FinishMatch(homeTeam, awayTeam);

        scoreboard.Matches.Should().NotContainKey((homeTeam, awayTeam))
            .And.HaveCount(0, "because the match should be removed from the scoreboard.");
    }
    
    [Fact]
    public void FinishMatch_WithValidMatch_RemovesCorrectMatchOnlyFromScoreboard()
    {
        var scoreboard = new FootballScoreboard();
        scoreboard.StartMatch("Germany", "Spain");
        scoreboard.StartMatch("England", "France");
        scoreboard.StartMatch("The Netherlands", "Argentina");
        
        scoreboard.FinishMatch("England", "France");

        var expectedMatches = new List<(string homeTeam, string awayTeam)>
        {
            ("Germany", "Spain"),
            ("The Netherlands", "Argentina")
        };
        scoreboard.Matches.Should().HaveCount(2, "because one of the games should be removed.");
        scoreboard.Matches.Keys.Should().BeEquivalentTo(expectedMatches, "because the remaining matches should be unchanged.");
    }
    
    [Fact]
    public void FinishMatch_WithNonExistentMatch_ThrowsException()
    {
        var scoreboard = new FootballScoreboard();
        
        var act = () => scoreboard.FinishMatch("Canada", "Belgium");

        act.Should().Throw<InvalidOperationException>().WithMessage("Match does not exist.");
    }
    
    [Fact]
    public void FinishMatch_WithOneTeamAlreadyPlaying_ThrowsException()
    {
        var scoreboard = new FootballScoreboard();
        scoreboard.StartMatch("Belgium", "Japan");
        
        var act = () => scoreboard.FinishMatch("Canada", "Belgium");

        act.Should().Throw<InvalidOperationException>().WithMessage("Match does not exist.");
        scoreboard.Matches.Should().ContainKey(("Belgium", "Japan"))
            .And.HaveCount(1);
    }
    
    [Fact]
    public void FinishMatch_WithEmptyTeamNames_ThrowsException()
    {
        var scoreboard = new FootballScoreboard();
        
        var act = () => scoreboard.FinishMatch("", "");

        act.Should().Throw<ArgumentException>().WithMessage("Invalid input.");
    }
    
    [Fact]
    public void FinishMatch_WithWhitespaceTeamNames_ThrowsException()
    {
        var scoreboard = new FootballScoreboard();
        
        var act = () => scoreboard.FinishMatch(" ", "     ");

        act.Should().Throw<ArgumentException>().WithMessage("Invalid input.");
    }

    [Fact]
    public void GetScoreboardSummary_WithValidMatchesAndDifferentScores_ReturnsSummary()
    {
        var scoreboard = new FootballScoreboard();
        scoreboard.StartMatch("Denmark", "Iceland");
        scoreboard.StartMatch("Poland", "Saudi Arabia");
        scoreboard.StartMatch("USA", "Wales");
        scoreboard.UpdateScore("Denmark", "Iceland", 5, 2);
        scoreboard.UpdateScore("Poland", "Saudi Arabia", 3, 0);
        scoreboard.UpdateScore("USA", "Wales", 2, 2);

        var result = scoreboard.GetScoreboardSummary();
        
        result.Should().NotBeNull("because the function should return a list of ongoing matches.")
            .And.HaveCount(3, "because the function should return all three matches.");
    }
    
    [Fact]
    public void GetScoreboardSummary_WithValidMatchesAndDifferentScores_ReturnsSummaryOrderedByScore()
    {
        var scoreboard = new FootballScoreboard();
        scoreboard.StartMatch("Denmark", "Iceland");
        scoreboard.StartMatch("Poland", "Saudi Arabia");
        scoreboard.StartMatch("USA", "Wales");
        scoreboard.UpdateScore("Denmark", "Iceland", 5, 2);
        scoreboard.UpdateScore("Poland", "Saudi Arabia", 3, 0);
        scoreboard.UpdateScore("USA", "Wales", 2, 2);

        var result = scoreboard.GetScoreboardSummary();
        
        result.Should().NotBeNull("because the function should return a list of ongoing matches")
            .And.HaveCount(3)
            .And.ContainItemsAssignableTo<FootballScoreboard.FootballMatch>()
            .And.SatisfyRespectively(
            first =>
            {
                first.HomeTeam.Should().Be("Denmark");
                first.AwayTeam.Should().Be("Iceland");
            },
            second =>
            {
                second.HomeTeam.Should().Be("USA");
                second.AwayTeam.Should().Be("Wales");
            },
            third =>
            {
                third.HomeTeam.Should().Be("Poland");
                third.AwayTeam.Should().Be("Saudi Arabia");
            });
    }
    
    [Fact]
    public void GetScoreboardSummary_WithValidMatchesAndSameTotalScores_ReturnsSummaryOrderedByStartTime()
    {
        var scoreboard = new FootballScoreboard();
        scoreboard.StartMatch("Denmark", "Iceland");
        Thread.Sleep(20);
        scoreboard.StartMatch("Poland", "Saudi Arabia");
        Thread.Sleep(20);
        scoreboard.StartMatch("USA", "Wales");
        scoreboard.UpdateScore("Denmark", "Iceland", 2, 2);
        scoreboard.UpdateScore("Poland", "Saudi Arabia", 3, 1);
        scoreboard.UpdateScore("USA", "Wales", 4, 0);

        var result = scoreboard.GetScoreboardSummary();
        
        result.Should().NotBeNull("because the function should return a list of ongoing matches")
            .And.HaveCount(3)
            .And.ContainItemsAssignableTo<FootballScoreboard.FootballMatch>()
            .And.SatisfyRespectively(
                first =>
                {
                    first.HomeTeam.Should().Be("USA");
                    first.AwayTeam.Should().Be("Wales");
                },
                second =>
                {
                    second.HomeTeam.Should().Be("Poland");
                    second.AwayTeam.Should().Be("Saudi Arabia");
                },
                third =>
                {
                    third.HomeTeam.Should().Be("Denmark");
                    third.AwayTeam.Should().Be("Iceland");
                });
    }
    
    [Fact]
    public void GetScoreboardSummary_WithValidMatchesAndMixedScores_ReturnsSummaryOrderedByScoreThenRecentStartTime()
    {
        var scoreboard = new FootballScoreboard();
        scoreboard.StartMatch("Mexico", "Canada");
        scoreboard.StartMatch("Spain", "Brazil");
        scoreboard.StartMatch("Germany", "France");
        scoreboard.StartMatch("Uruguay", "Italy");
        scoreboard.StartMatch("Argentina", "Australia");
        scoreboard.UpdateScore("Mexico", "Canada", 0, 5);
        scoreboard.UpdateScore("Spain", "Brazil", 10, 2);
        scoreboard.UpdateScore("Germany", "France", 2, 2);
        scoreboard.UpdateScore("Uruguay", "Italy", 6, 6);
        scoreboard.UpdateScore("Argentina", "Australia", 3, 1);
        
        var result = scoreboard.GetScoreboardSummary();
        
        result.Should().NotBeNull("because the function should return a list of ongoing matches")
            .And.HaveCount(5)
            .And.ContainItemsAssignableTo<FootballScoreboard.FootballMatch>()
            .And.SatisfyRespectively(
                first =>
                {
                    first.HomeTeam.Should().Be("Uruguay");
                    first.AwayTeam.Should().Be("Italy");
                },
                second =>
                {
                    second.HomeTeam.Should().Be("Spain");
                    second.AwayTeam.Should().Be("Brazil");
                },
                third =>
                {
                    third.HomeTeam.Should().Be("Mexico");
                    third.AwayTeam.Should().Be("Canada");
                },
                fourth =>
                {
                    fourth.HomeTeam.Should().Be("Argentina");
                    fourth.AwayTeam.Should().Be("Australia");
                },
                fifth =>
                {
                    fifth.HomeTeam.Should().Be("Germany");
                    fifth.AwayTeam.Should().Be("France");
                });
    }
    
    [Fact]
    public void GetScoreboardSummary_WithNoMatches_ReturnsEmptyList()
    {
        var scoreboard = new FootballScoreboard();

        var result = scoreboard.GetScoreboardSummary();

        result.Should().BeEmpty("because there are no matches in the scoreboard");
    }
}