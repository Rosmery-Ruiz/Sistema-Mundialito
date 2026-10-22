using Mundialito.Application.Common;
using Mundialito.Application.Common.Interfaces;

namespace Mundialito.Application.Match.Queries
{
    public class GetMatchesQueryHandler
    {
        private readonly IMatchQueryRepository _matchQueryRepository;

        public GetMatchesQueryHandler(
            IMatchQueryRepository matchQueryRepository)
        {
            _matchQueryRepository = matchQueryRepository;
        }

        public async Task<PagedResult<Mundialito.Domain.Entities.Match>> HandleAsync(
            GetMatchesQuery query,
            CancellationToken cancellationToken = default)
        {
            return await _matchQueryRepository.GetAllAsync(
                query,
                cancellationToken);
        }
    }
}