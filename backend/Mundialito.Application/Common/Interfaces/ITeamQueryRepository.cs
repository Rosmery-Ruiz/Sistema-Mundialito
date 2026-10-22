using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mundialito.Domain.Entities;
using Mundialito.Application.Team.Queries;

namespace Mundialito.Application.Common.Interfaces
{
    public interface ITeamQueryRepository
    {
        Task<PagedResult<Mundialito.Domain.Entities.Team>> GetAllAsync(
            GetTeamsQuery query,
            CancellationToken cancellationToken = default);
    }
}