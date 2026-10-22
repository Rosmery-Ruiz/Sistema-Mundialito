using Dapper;
using Mundialito.Application.Common;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Player.Queries;

namespace Mundialito.Infrastructure.Queries
{
    public class PlayerQueryRepository : IPlayerQueryRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PlayerQueryRepository(
            IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PagedResult<Mundialito.Domain.Entities.Player>> GetAllAsync(
            GetPlayersQuery query,
            CancellationToken cancellationToken = default)
        {
            using var connection = _connectionFactory.CreateConnection();

            var offset = (query.PageNumber - 1) * query.PageSize;

            var sortColumn = query.SortBy?.ToLower() switch
            {
                "name" => "Name",
                "shirtnumber" => "ShirtNumber",
                "teamid" => "TeamId",
                _ => "Id"
            };

            var sortDirection = query.SortDirection?.ToLower() == "desc"
                ? "DESC"
                : "ASC";
            var sql = $"""
                SELECT Id, TeamId, Name, ShirtNumber
                FROM Players
                WHERE (@TeamId IS NULL OR TeamId = @TeamId)
                  AND (@Name IS NULL OR Name LIKE '%' + @Name + '%')
                ORDER BY {sortColumn} {sortDirection}
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(*)
                FROM Players
                WHERE (@TeamId IS NULL OR TeamId = @TeamId)
                  AND (@Name IS NULL OR Name LIKE '%' + @Name + '%');
                """;

            using var multi = await connection.QueryMultipleAsync(
                new CommandDefinition(
                    sql,
                    new
                    {
                        query.TeamId,
                        query.Name,
                        Offset = offset,
                        query.PageSize
                    },
                    cancellationToken: cancellationToken));

            var players = await multi
                .ReadAsync<Mundialito.Domain.Entities.Player>();

            var totalCount = await multi
                .ReadSingleAsync<int>();

            return new PagedResult<Mundialito.Domain.Entities.Player>
            {
                Data = players,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalRecords = totalCount
            };
        }
    }
}