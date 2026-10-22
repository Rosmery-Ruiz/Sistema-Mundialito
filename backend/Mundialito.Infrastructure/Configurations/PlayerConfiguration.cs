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
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder
            .HasOne<Team>() /*un jugador pertenece a un solo equipo*/
            .WithMany()    /*un equipo puede estar relacionado con muchos jugadores*/
            .HasForeignKey(player => player.TeamId) /*La propiedad TeamId de Player es la que conecta al jugador con Team.*/
            .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
