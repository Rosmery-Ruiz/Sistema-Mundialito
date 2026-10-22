using Mundialito.Application.Standings.Dtos;
using Mundialito.Application.Standings.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Common.Interfaces
{
    public interface IStandingsQueryRepository
    {
        Task<PagedResult<StandingDto>> GetStandingsAsync(
       GetStandingsQuery query,
       CancellationToken cancellationToken = default);
    }
}