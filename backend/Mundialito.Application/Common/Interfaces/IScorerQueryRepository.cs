using Mundialito.Application.Scorers.Dtos;
using Mundialito.Application.Scorers.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Common.Interfaces
{
    public interface IScorerQueryRepository
    {
        Task<PagedResult<ScorerDto>> GetScorersAsync(
        GetScorersQuery query,
        CancellationToken cancellationToken = default);
    }
}