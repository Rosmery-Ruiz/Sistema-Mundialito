using Microsoft.EntityFrameworkCore;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Domain.Entities;

namespace Mundialito.Infrastructure.Persistence.Repositories
{
    public class IdempotencyRepository : IIdempotencyRepository
    {
        private readonly MundialitoDbContext _context;

        public IdempotencyRepository(MundialitoDbContext context)
        {
            _context = context;
        }

        public async Task<IdempotencyRecord?> GetByKeyAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            return await _context.IdempotencyRecords
                .FirstOrDefaultAsync(
                    x => x.Key == key,
                    cancellationToken);
        }

        public async Task AddAsync(
            IdempotencyRecord record,
            CancellationToken cancellationToken = default)
        {
            await _context.IdempotencyRecords.AddAsync(
                record,
                cancellationToken);
        }
    }
}