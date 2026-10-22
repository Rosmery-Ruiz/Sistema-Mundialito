using Mundialito.Application.Common;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Standings.Dtos;

namespace Mundialito.Application.Standings.Queries
{
    public class GetStandingsQueryHandler
    {
        private readonly IStandingsQueryRepository _standingsQueryRepository;

        public GetStandingsQueryHandler(
            IStandingsQueryRepository standingsQueryRepository)
        {
            _standingsQueryRepository = standingsQueryRepository;
        }

        public async Task<PagedResult<StandingDto>> HandleAsync(
            GetStandingsQuery query,
            CancellationToken cancellationToken = default)
        {
            return await _standingsQueryRepository.GetStandingsAsync(
    query,
    cancellationToken);
        }
    }
}