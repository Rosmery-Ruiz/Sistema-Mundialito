using Mundialito.Application.Common.Interfaces;
using Mundialito.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Infrastructure.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly MundialitoDbContext _context;

        public PlayerRepository(MundialitoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            Mundialito.Domain.Entities.Player player,
            CancellationToken cancellationToken = default)
        {
            await _context.Players.AddAsync(
                player,
                cancellationToken);
        }
    }
}