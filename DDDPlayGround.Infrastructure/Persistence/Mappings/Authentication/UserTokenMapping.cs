using DDDPlayGround.Domain.Constants;
using DDDPlayGround.Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDPlayGround.Infrastructure.Persistence.Mappings.Authentication
{
    internal class UserTokenMapping : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("UserTokens", SchemaName.Identity);

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Token)
                .IsRequired();

            builder.Property(t => t.Expires)
                .IsRequired();

            builder.Property(t => t.IsRevoked)
                .IsRequired();
        }
    }
}