using Mundialito.Application.Common;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Scorers.Dtos;


namespace Mundialito.Application.Scorers.Queries
{
    public class GetScorersQueryHandler
    {
        private readonly IScorerQueryRepository _scorerQueryRepository;

        public GetScorersQueryHandler(
            IScorerQueryRepository scorerQueryRepository)
        {
            _scorerQueryRepository = scorerQueryRepository;
        }

        public async Task<PagedResult<ScorerDto>> HandleAsync(
    GetScorersQuery query,
    CancellationToken cancellationToken = default)
        {
            return await _scorerQueryRepository.GetScorersAsync(
     query,
     cancellationToken);
        }
    }
}