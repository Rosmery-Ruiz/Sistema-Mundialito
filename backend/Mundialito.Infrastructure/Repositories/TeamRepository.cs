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
    public class TeamRepository : ITeamRepository
    {
        private readonly MundialitoDbContext _context;

        public TeamRepository(MundialitoDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(
    Mundialito.Domain.Entities.Team team,
    CancellationToken cancellationToken = default)
        {
            await _context.Teams.AddAsync(team, cancellationToken);
        }
        public async Task<Mundialito.Domain.Entities.Team?> GetByIdAsync(
    int id,
    CancellationToken cancellationToken = default)
        {
            return await _context.Teams
                .FirstOrDefaultAsync(
                    team => team.Id == id,
                    cancellationToken);
        }

        public void Delete(
           Mundialito.Domain.Entities.Team team)
        {
            _context.Teams.Remove(team);
        }
    }
}
