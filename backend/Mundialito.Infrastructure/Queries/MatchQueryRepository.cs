using Dapper;
using Mundialito.Application.Common;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Match.Queries;

namespace Mundialito.Infrastructure.Queries
{
    public class MatchQueryRepository : IMatchQueryRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public MatchQueryRepository(
            IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PagedResult<Mundialito.Domain.Entities.Match>> GetAllAsync(
            GetMatchesQuery query,
            CancellationToken cancellationToken = default)
        {
            using var connection = _connectionFactory.CreateConnection();

            var offset = (query.PageNumber - 1) * query.PageSize;
            var sortColumn = query.SortBy?.ToLower() switch
            {
                "matchdate" => "MatchDate",
                "hometeamid" => "HomeTeamId",
                "awayteamid" => "AwayTeamId",
                "status" => "Status",
                _ => "MatchDate"
            };

            var sortDirection = query.SortDirection?.ToLower() == "desc"
                ? "DESC"
                : "ASC";
            var sql = $"""
                SELECT
                    Id,
                    HomeTeamId,
                    AwayTeamId,
                    MatchDate,
                    HomeScore,
                    AwayScore,
                    Status
                FROM Matches
                WHERE (@MatchDate IS NULL OR CAST(MatchDate AS date) = CAST(@MatchDate AS date))
                  AND (@TeamId IS NULL OR HomeTeamId = @TeamId OR AwayTeamId = @TeamId)
                  AND (@Status IS NULL OR Status = @Status)
                ORDER BY {sortColumn} {sortDirection}
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(*)
                FROM Matches
                WHERE (@MatchDate IS NULL OR CAST(MatchDate AS date) = CAST(@MatchDate AS date))
                  AND (@TeamId IS NULL OR HomeTeamId = @TeamId OR AwayTeamId = @TeamId)
                  AND (@Status IS NULL OR Status = @Status);
                """;

            using var multi = await connection.QueryMultipleAsync(
                new CommandDefinition(
                    sql,
                    new
                    {
                        query.MatchDate,
                        query.TeamId,
                        query.Status,
                        Offset = offset,
                        query.PageSize
                    },
                    cancellationToken: cancellationToken));

            var matches = await multi
                .ReadAsync<Mundialito.Domain.Entities.Match>();

            var totalCount = await multi
                .ReadSingleAsync<int>();

            return new PagedResult<Mundialito.Domain.Entities.Match>
            {
                Data = matches,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalRecords = totalCount
            };
        }
    }
}