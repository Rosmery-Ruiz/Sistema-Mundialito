using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Common.Interfaces
{
    public interface IMatchRepository
    {
        Task AddAsync(
            Mundialito.Domain.Entities.Match match,
            CancellationToken cancellationToken = default);
        Task<Mundialito.Domain.Entities.Match?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
