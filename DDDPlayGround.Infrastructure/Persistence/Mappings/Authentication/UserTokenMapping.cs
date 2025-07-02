using DDDPlayGround.Domain.Constants;
using DDDPlayGround.Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDPlayGround.Infrastructure.Persistence.Mappings.Authentication
{
    public class UserTokenMapping : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("UserTokens", SchemaName.Identity);

            builder.HasKey(ut => ut.Id);

            builder.Property(ut => ut.Token)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(ut => ut.CreatedByIp)
                   .HasMaxLength(45); 

            builder.Property(ut => ut.RevokedByIp)
                   .HasMaxLength(45);

            builder.Property(ut => ut.ReplacedByToken)
                   .HasMaxLength(500);

            builder.HasOne(ut => ut.User)
                   .WithMany(u => u.Tokens)
                   .HasForeignKey(ut => ut.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}