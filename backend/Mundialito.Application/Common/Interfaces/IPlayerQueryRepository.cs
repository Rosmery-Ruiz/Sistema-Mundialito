using Mundialito.Application.Player.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Common.Interfaces
{
    public interface IPlayerQueryRepository
    {
        Task<PagedResult<Mundialito.Domain.Entities.Player>> GetAllAsync(
            GetPlayersQuery query,
            CancellationToken cancellationToken = default);
    }
}