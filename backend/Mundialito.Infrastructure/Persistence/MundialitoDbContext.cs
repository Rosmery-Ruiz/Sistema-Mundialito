using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mundialito.Domain.Entities;

namespace Mundialito.Infrastructure.Persistence
{
    public class MundialitoDbContext: DbContext
    {
        public MundialitoDbContext(DbContextOptions<MundialitoDbContext> options) : base(options)

        { }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<IdempotencyRecord> IdempotencyRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(MundialitoDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }


}
