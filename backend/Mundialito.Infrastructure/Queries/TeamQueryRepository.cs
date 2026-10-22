using Dapper;
using Mundialito.Application.Common;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Team.Queries;

namespace Mundialito.Infrastructure.Queries
{
    public class TeamQueryRepository : ITeamQueryRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TeamQueryRepository(
            IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PagedResult<Mundialito.Domain.Entities.Team>> GetAllAsync(
            GetTeamsQuery query,
            CancellationToken cancellationToken = default)
        {
            using var connection = _connectionFactory.CreateConnection();

            var offset = (query.PageNumber - 1) * query.PageSize;

            var sortColumn = query.SortBy.ToLower() switch
            {
                "name" => "Name",
                _ => "Id"
            };

            var sortDirection = query.SortDirection.ToLower() == "desc"
                ? "DESC"
                : "ASC";

            var sql = $"""
                SELECT Id, Name
                FROM Teams
                WHERE (@Name IS NULL OR Name LIKE '%' + @Name + '%')
                ORDER BY {sortColumn} {sortDirection}
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(*)
                FROM Teams
                WHERE (@Name IS NULL OR Name LIKE '%' + @Name + '%');
                """;

            using var multi = await connection.QueryMultipleAsync(
                new CommandDefinition(
                    sql,
                    new
                    {
                        query.Name,
                        Offset = offset,
                        query.PageSize
                    },
                    cancellationToken: cancellationToken));

            var teams = await multi
                .ReadAsync<Mundialito.Domain.Entities.Team>();

            var totalCount = await multi
                .ReadSingleAsync<int>();

            return new PagedResult<Mundialito.Domain.Entities.Team>
            {
                Data = teams,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalRecords = totalCount
            };
        }
    }
}