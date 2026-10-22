using Mundialito.Application.Match.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Common.Interfaces
{
    public interface IMatchQueryRepository
    {
        Task<PagedResult<Mundialito.Domain.Entities.Match>> GetAllAsync(
            GetMatchesQuery query,
            CancellationToken cancellationToken = default);
    }
}