# Scoreboard Library

## Overview
This is a simple scoreboard library used to manage football matches in a live scoreboard. It has the ability to start and finish matches, update scores, and return a detailed summary of ongoing matches. 

The solution is developed using **C#** with the **.NET 8** framework, and the TDD method to ensure code quality and proper and comprehensive unit tests. The test project is based on the **XUnit** framework and use the **FluentAssertion** library.


## Features
- Start a new football match with specified home and away teams.
- Update the score in a match by providing an absolute score.
- Finish a match and remove it from scoreboard.
- Get a summary of all matches on scoreboard currently in progress.

## Usage
```c#
var scoreboard = new FootballScoreboard();

scoreboard.StartMatch("Team A", "Team B");

scoreboard.UpdateScore("Team A", "Team B", 1, 0);

scoreboard.FinishMatch("Team A", "Team B");

var summary = scoreboard.GetScoreboardSummary();
```

## Point of considerations
As this was part of a coding challenge, there were several considerations and assumptions being made on the spot, especially when weighing **simplicity** vs **showcasing skills**. I wanted the solution to be simple and easy to understand but at the same time follow OO best-practices.

#### These are some inner discussions I had while developing:
- **Interface**. Was the use of an interface necessary to fulfill all the requirements? No. Did it make the solution more complex? Perhaps, yes. However, using an interface in this instance is always good practice. It provides readability and maintainability and makes the Scoreboard scalable and future proof in case one were to add different types of scoreboards. It would also be beneficial if one were to test a service dependent on the scoreboard and needed to mock behaviour.
- **Threading**. This solution is not considered to be multi-thread safe. This could lead to inconsistent data and deadlocks. I considered this not to be a part of the scope of this solution, in order to maintain simplicity. In a production environment one could consider locking data operations or introducing an async pattern.
- **Not full encapsulation**. As of now the unit tests are allowed direct access to the internal state of the scoreboard. The Matches-property is public and the unit test can access the data by doing `scoreboard.Matches`. To ensure full encapsulation one could make the Matches collection and the FootballMatch class private and add a new method in the FootballScoreboard class that returns a DTO. This would be ideal, but due to simplicity and the fear of over-engineering, this was omitted.
- **Dictionary**. Midways through developing, the Matches data type changed from a List to a Dictionary. After noticing that several methods needed to perform lookups, I opted for a data type more beneficial for this operation. The key of the dictionary is a tuple of (HomeTeam, AwayTeam). This improves performance of lookup of specific matches, especially if Matches would increase. This does not sacrifice simplicity and at the same time makes the solution more scalable and robust.
- **UpdateScore()**. The function UpdateScore needs homeTeam and awayTeam parameters. This choice was made to avoid the introduction of a unique match ID property or the need of a match instance. In a production environment one would most likely prefer a unique match ID, but too keep the solution (somewhat) simple, this was omitted. In this scope the combination of homeTeam and awayTeam serve as the match identifier.