using Dapper;
using Mundialito.Application.Common;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Standings.Dtos;
using Mundialito.Application.Standings.Queries;
using Mundialito.Application.Common;


namespace Mundialito.Infrastructure.Queries
{
    public class StandingsQueryRepository : IStandingsQueryRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public StandingsQueryRepository(
            IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PagedResult<StandingDto>> GetStandingsAsync(
     GetStandingsQuery query,
    CancellationToken cancellationToken = default)
        {
            using var connection = _connectionFactory.CreateConnection();

            var offset = (query.PageNumber - 1) * query.PageSize;

            var sortColumn = query.SortBy?.ToLower() switch
            {
                "teamname" => "TeamName",
                "played" => "Played",
                "won" => "Won",
                "drawn" => "Drawn",
                "lost" => "Lost",
                "goalsfor" => "GoalsFor",
                "goalsagainst" => "GoalsAgainst",
                "goaldifference" => "GoalDifference",
                "points" => "Points",
                _ => "Points"
            };

            var sortDirection = query.SortDirection?.ToLower() == "asc"
                ? "ASC"
                : "DESC";

            var orderBy = sortColumn == "Points" && sortDirection == "DESC"
    ? "Points DESC, GoalDifference DESC, GoalsFor DESC"
    : $"{sortColumn} {sortDirection}";

            var countSql = """
    SELECT COUNT(*)
    FROM Teams;
    """;

            var sql = $"""
        WITH Standings AS
        (
            SELECT
                t.Id AS TeamId,
                t.Name AS TeamName,

                COUNT(m.Id) AS Played,

                SUM(CASE
                    WHEN m.Id IS NULL THEN 0
                    WHEN m.HomeTeamId = t.Id
                         AND m.HomeScore > m.AwayScore THEN 1
                    WHEN m.AwayTeamId = t.Id
                         AND m.AwayScore > m.HomeScore THEN 1
                    ELSE 0
                END) AS Won,

                SUM(CASE
                    WHEN m.Id IS NOT NULL
                         AND m.HomeScore = m.AwayScore THEN 1
                    ELSE 0
                END) AS Drawn,

                SUM(CASE
                    WHEN m.Id IS NULL THEN 0
                    WHEN m.HomeTeamId = t.Id
                         AND m.HomeScore < m.AwayScore THEN 1
                    WHEN m.AwayTeamId = t.Id
                         AND m.AwayScore < m.HomeScore THEN 1
                    ELSE 0
                END) AS Lost,

                SUM(CASE
                    WHEN m.HomeTeamId = t.Id THEN m.HomeScore
                    WHEN m.AwayTeamId = t.Id THEN m.AwayScore
                    ELSE 0
                END) AS GoalsFor,

                SUM(CASE
                    WHEN m.HomeTeamId = t.Id THEN m.AwayScore
                    WHEN m.AwayTeamId = t.Id THEN m.HomeScore
                    ELSE 0
                END) AS GoalsAgainst

            FROM Teams t

            LEFT JOIN Matches m
                ON (m.HomeTeamId = t.Id OR m.AwayTeamId = t.Id)
                AND m.Status = 'Finished'

            GROUP BY
                t.Id,
                t.Name
        )

        SELECT
            TeamId,
            TeamName,
            Played,
            Won,
            Drawn,
            Lost,
            GoalsFor,
            GoalsAgainst,
            GoalsFor - GoalsAgainst AS GoalDifference,
            (Won * 3) + Drawn AS Points

      FROM Standings

ORDER BY {orderBy}
OFFSET @Offset ROWS
FETCH NEXT @PageSize ROWS ONLY;
""";
            var totalRecords = await connection.ExecuteScalarAsync<int>(
    new CommandDefinition(
        countSql,
        cancellationToken: cancellationToken));

            var standings = await connection.QueryAsync<StandingDto>(
     new CommandDefinition(
         sql,
         new
         {
             Offset = offset,
             PageSize = query.PageSize
         },
         cancellationToken: cancellationToken));

            return new PagedResult<StandingDto>
            {
                Data = standings,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalRecords = totalRecords
            };
        }
    }
}