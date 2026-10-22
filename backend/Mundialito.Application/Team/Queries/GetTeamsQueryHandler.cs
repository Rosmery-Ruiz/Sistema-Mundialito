using Mundialito.Application.Common;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Team.Queries
{
    public class GetTeamsQueryHandler
    {
        private readonly ITeamQueryRepository _teamQueryRepository;

        public GetTeamsQueryHandler(
            ITeamQueryRepository teamQueryRepository)
        {
            _teamQueryRepository = teamQueryRepository;
        }

        public async Task<PagedResult<Mundialito.Domain.Entities.Team>> HandleAsync(
            GetTeamsQuery query,
            CancellationToken cancellationToken = default)
        {
            return await _teamQueryRepository.GetAllAsync(
                query,
                cancellationToken);
        }
    }
}