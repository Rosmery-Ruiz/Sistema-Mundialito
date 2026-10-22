using Microsoft.EntityFrameworkCore;

namespace Mundialito.Infrastructure.Persistence.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(
            MundialitoDbContext context,
            CancellationToken cancellationToken = default)
        {
            var hasTeams = await context.Teams.AnyAsync(cancellationToken);

            if (hasTeams)
            {
                return;
            }
            var teams = new[]
{
    new Domain.Entities.Team { Name = "Tigres" },
    new Domain.Entities.Team { Name = "Leones" },
    new Domain.Entities.Team { Name = "Águilas" },
    new Domain.Entities.Team { Name = "Toros" }
};

            await context.Teams.AddRangeAsync(teams, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            var players = new[]
{
    // Tigres
    new Domain.Entities.Player { Name = "Jugador 1", ShirtNumber = 1, TeamId = teams[0].Id },
    new Domain.Entities.Player { Name = "Jugador 2", ShirtNumber = 2, TeamId = teams[0].Id },
    new Domain.Entities.Player { Name = "Jugador 3", ShirtNumber = 3, TeamId = teams[0].Id },
    new Domain.Entities.Player { Name = "Jugador 4", ShirtNumber = 4, TeamId = teams[0].Id },
    new Domain.Entities.Player { Name = "Jugador 5", ShirtNumber = 5, TeamId = teams[0].Id },

    // Leones
    new Domain.Entities.Player { Name = "Jugador 6", ShirtNumber = 1, TeamId = teams[1].Id },
    new Domain.Entities.Player { Name = "Jugador 7", ShirtNumber = 2, TeamId = teams[1].Id },
    new Domain.Entities.Player { Name = "Jugador 8", ShirtNumber = 3, TeamId = teams[1].Id },
    new Domain.Entities.Player { Name = "Jugador 9", ShirtNumber = 4, TeamId = teams[1].Id },
    new Domain.Entities.Player { Name = "Jugador 10", ShirtNumber = 5, TeamId = teams[1].Id },

    // Águilas
    new Domain.Entities.Player { Name = "Jugador 11", ShirtNumber = 1, TeamId = teams[2].Id },
    new Domain.Entities.Player { Name = "Jugador 12", ShirtNumber = 2, TeamId = teams[2].Id },
    new Domain.Entities.Player { Name = "Jugador 13", ShirtNumber = 3, TeamId = teams[2].Id },
    new Domain.Entities.Player { Name = "Jugador 14", ShirtNumber = 4, TeamId = teams[2].Id },
    new Domain.Entities.Player { Name = "Jugador 15", ShirtNumber = 5, TeamId = teams[2].Id },

    // Toros
    new Domain.Entities.Player { Name = "Jugador 16", ShirtNumber = 1, TeamId = teams[3].Id },
    new Domain.Entities.Player { Name = "Jugador 17", ShirtNumber = 2, TeamId = teams[3].Id },
    new Domain.Entities.Player { Name = "Jugador 18", ShirtNumber = 3, TeamId = teams[3].Id },
    new Domain.Entities.Player { Name = "Jugador 19", ShirtNumber = 4, TeamId = teams[3].Id },
    new Domain.Entities.Player { Name = "Jugador 20", ShirtNumber = 5, TeamId = teams[3].Id }
};

            await context.Players.AddRangeAsync(players, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var matches = new[]
{
    new Domain.Entities.Match
    {
        HomeTeamId = teams[0].Id,
        AwayTeamId = teams[1].Id,
        MatchDate = DateTime.UtcNow.AddDays(-6),
        HomeScore = 2,
        AwayScore = 1,
        Status = "Finished"
    },
    new Domain.Entities.Match
    {
        HomeTeamId = teams[2].Id,
        AwayTeamId = teams[3].Id,
        MatchDate = DateTime.UtcNow.AddDays(-5),
        HomeScore = 1,
        AwayScore = 1,
        Status = "Finished"
    },
    new Domain.Entities.Match
    {
        HomeTeamId = teams[0].Id,
        AwayTeamId = teams[2].Id,
        MatchDate = DateTime.UtcNow.AddDays(-4),
        HomeScore = 0,
        AwayScore = 2,
        Status = "Finished"
    },
    new Domain.Entities.Match
    {
        HomeTeamId = teams[1].Id,
        AwayTeamId = teams[3].Id,
        MatchDate = DateTime.UtcNow.AddDays(1),
        HomeScore = null,
        AwayScore = null,
        Status = "Scheduled"
    },
    new Domain.Entities.Match
    {
        HomeTeamId = teams[0].Id,
        AwayTeamId = teams[3].Id,
        MatchDate = DateTime.UtcNow.AddDays(2),
        HomeScore = null,
        AwayScore = null,
        Status = "Scheduled"
    },
    new Domain.Entities.Match
    {
        HomeTeamId = teams[1].Id,
        AwayTeamId = teams[2].Id,
        MatchDate = DateTime.UtcNow.AddDays(3),
        HomeScore = null,
        AwayScore = null,
        Status = "Scheduled"
    }
};

            await context.Matches.AddRangeAsync(matches, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var goals = new[]
{
    // Match 1: Tigres 2 - 1 Leones
    new Domain.Entities.Goal
    {
        MatchId = matches[0].Id,
        PlayerId = players[0].Id
    },
    new Domain.Entities.Goal
    {
        MatchId = matches[0].Id,
        PlayerId = players[1].Id
    },
    new Domain.Entities.Goal
    {
        MatchId = matches[0].Id,
        PlayerId = players[5].Id
    },

    // Match 2: Águilas 1 - 1 Toros
    new Domain.Entities.Goal
    {
        MatchId = matches[1].Id,
        PlayerId = players[10].Id
    },
    new Domain.Entities.Goal
    {
        MatchId = matches[1].Id,
        PlayerId = players[15].Id
    },

    // Match 3: Tigres 0 - 2 Águilas
    new Domain.Entities.Goal
    {
        MatchId = matches[2].Id,
        PlayerId = players[11].Id
    },
    new Domain.Entities.Goal
    {
        MatchId = matches[2].Id,
        PlayerId = players[12].Id
    }
};

            await context.Goals.AddRangeAsync(goals, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }


    }
}