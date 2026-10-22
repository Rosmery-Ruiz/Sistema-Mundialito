using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Common.Interfaces
{
    public interface ITeamRepository
    {
        Task AddAsync(
           Mundialito.Domain.Entities.Team team,
           CancellationToken cancellationToken = default);

        Task<Mundialito.Domain.Entities.Team?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        void Delete(
           Mundialito.Domain.Entities.Team team);
    }
}
