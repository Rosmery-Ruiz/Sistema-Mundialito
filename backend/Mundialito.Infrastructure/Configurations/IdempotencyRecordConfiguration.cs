using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mundialito.Domain.Entities;

namespace Mundialito.Infrastructure.Persistence.Configurations
{
    public class IdempotencyRecordConfiguration
        : IEntityTypeConfiguration<IdempotencyRecord>
    {
        public void Configure(EntityTypeBuilder<IdempotencyRecord> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Key)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Key)
                .IsUnique();

            builder.Property(x => x.ResponseBody)
                .IsRequired();

            builder.Property(x => x.StatusCode)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();
        }
    }
}