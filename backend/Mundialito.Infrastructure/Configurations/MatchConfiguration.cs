using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Infrastructure.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
             builder
                    .HasOne<Team>()              /* El partido tiene relacion con un equipo*/
                    .WithMany()                  /* Un equipo puede aparecer en muchos partidos*/
                    .HasForeignKey(match => match.HomeTeamId) /*La conexion se hace mediante HomeTeamId identifica al equipo local*/
                    .OnDelete(DeleteBehavior.Restrict); /*Si un equipo ya está siendo usado en un partido, no se podrá eliminar así por así. Así evitamos borrar accidentalmente partidos relacionados.*/


            builder
                    .HasOne<Team>()   /* El partido tiene relacion con un equipo*/
                    .WithMany()        /* Un equipo puede aparecer en muchos partidos*/
                    .HasForeignKey(match => match.AwayTeamId) /*La conexion se hace mediante HomeTeamId identifica al equipo visitante*/
                    .OnDelete(DeleteBehavior.Restrict); /*Si un equipo ya está siendo usado en un partido, no se podrá eliminar así por así. Así evitamos borrar accidentalmente partidos relacionados.*/
        }
    }
}
