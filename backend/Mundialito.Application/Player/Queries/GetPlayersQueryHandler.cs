using Mundialito.Application.Common.Interfaces;
using Mundialito.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Player.Queries
{
    public class GetPlayersQueryHandler
    {
        private readonly IPlayerQueryRepository _playerQueryRepository;

        public GetPlayersQueryHandler(
            IPlayerQueryRepository playerQueryRepository)
        {
            _playerQueryRepository = playerQueryRepository;
        }

        public async Task<PagedResult<Mundialito.Domain.Entities.Player>> HandleAsync(
            GetPlayersQuery query,
            CancellationToken cancellationToken = default)
        {
            return await _playerQueryRepository.GetAllAsync(
                query,
                cancellationToken);
        }
    }
}