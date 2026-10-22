using Microsoft.EntityFrameworkCore;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Infrastructure.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly MundialitoDbContext _context;

        public MatchRepository(MundialitoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            Mundialito.Domain.Entities.Match match,
            CancellationToken cancellationToken = default)
        {
            await _context.Matches.AddAsync(
                match,
                cancellationToken);
        }

        public async Task<Mundialito.Domain.Entities.Match?> GetByIdAsync(
    int id,
    CancellationToken cancellationToken = default)
        {
            return await _context.Matches
                .FirstOrDefaultAsync(
                    match => match.Id == id,
                    cancellationToken);
        }
    }
}