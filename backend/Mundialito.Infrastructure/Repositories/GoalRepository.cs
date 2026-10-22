using Mundialito.Application.Common.Interfaces;
using Mundialito.Domain.Entities;
using Mundialito.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Infrastructure.Repositories
{
    public class GoalRepository : IGoalRepository
    {
        private readonly MundialitoDbContext _context;

        public GoalRepository(MundialitoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            Goal goal,
            CancellationToken cancellationToken = default)
        {
            await _context.Goals.AddAsync(goal, cancellationToken);
        }
    }
}