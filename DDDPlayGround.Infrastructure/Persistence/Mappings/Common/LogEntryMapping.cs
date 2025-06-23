using DDDPlayGround.Domain.Entities.Common;
using DDDPlayGround.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDPlayGround.Infrastructure.Persistence.Mappings.Common
{
    public class LogEntryMapping : IEntityTypeConfiguration<LogEntry>
    {
        public void Configure(EntityTypeBuilder<LogEntry> builder)
        {
            builder.ToTable("Logs", SchemaName.Log);

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Level)
                .HasConversion<string>() // store enum as string for readability
                .IsRequired();

            builder.Property(e => e.Message)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(e => e.Exception)
                .HasMaxLength(4000);

            builder.Property(e => e.CorrelationId)
                .HasMaxLength(100);

            builder.Property(e => e.User)
                .HasMaxLength(200);

            builder.HasIndex(e => e.Level);

            builder.HasIndex(e => e.CorrelationId);
        }
    }
}
