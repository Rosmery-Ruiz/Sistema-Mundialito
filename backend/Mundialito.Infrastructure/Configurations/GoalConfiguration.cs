using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Infrastructure.Configurations
{
    public class GoalConfiguration: IEntityTypeConfiguration<Goal>
    {
        public void Configure(EntityTypeBuilder<Goal> builder)
        {
            builder
                  .HasOne<Match>()
                  .WithMany()
                  .HasForeignKey(goal => goal.MatchId);

            builder
                  .HasOne<Player>()
                  .WithMany()
                  .HasForeignKey(goal => goal.PlayerId)
                  .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
