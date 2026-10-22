using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Application.Common.Interfaces
{
    public interface IPlayerRepository
    {
        Task AddAsync(
            Mundialito.Domain.Entities.Player player,
            CancellationToken cancellationToken = default);
    }
}