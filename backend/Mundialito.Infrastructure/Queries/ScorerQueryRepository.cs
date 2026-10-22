using Dapper;
using Mundialito.Application.Common;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Scorers.Dtos;
using Mundialito.Application.Scorers.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Mundialito.Infrastructure.Queries
{
    public class ScorerQueryRepository : IScorerQueryRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ScorerQueryRepository(
            IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PagedResult<ScorerDto>> GetScorersAsync(
     GetScorersQuery query,
     CancellationToken cancellationToken = default)
        {
            using var connection = _connectionFactory.CreateConnection();

            var offset = (query.PageNumber - 1) * query.PageSize;
            var sortColumn = query.SortBy?.ToLower() switch
            {
                "playername" => "PlayerName",
                "teamname" => "TeamName",
                "goals" => "Goals",
                _ => "Goals"
            };

            var sortDirection = query.SortDirection?.ToLower() == "asc"
                ? "ASC"
                : "DESC";

            var countSql = """

    SELECT COUNT(DISTINCT PlayerId)
    FROM Goals;
    """;

            var sql = $"""
                SELECT
                    p.Id AS PlayerId,
                    p.Name AS PlayerName,
                    t.Id AS TeamId,
                    t.Name AS TeamName,
                    COUNT(g.Id) AS Goals
                FROM Goals g
                INNER JOIN Players p
                    ON g.PlayerId = p.Id
                INNER JOIN Teams t
                    ON p.TeamId = t.Id
                GROUP BY
                    p.Id,
                    p.Name,
                    t.Id,
                    t.Name
               ORDER BY {sortColumn} {sortDirection}
OFFSET @Offset ROWS
FETCH NEXT @PageSize ROWS ONLY;
""";
            var totalRecords = await connection.ExecuteScalarAsync<int>(
    new CommandDefinition(
        countSql,
        cancellationToken: cancellationToken));

            var scorers = await connection.QueryAsync<ScorerDto>(
     new CommandDefinition(
         sql,
         new
         {
             Offset = offset,
             PageSize = query.PageSize
         },
         cancellationToken: cancellationToken));

            return new PagedResult<ScorerDto>
            {
                Data = scorers,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalRecords = totalRecords
            };
        }
    }
}