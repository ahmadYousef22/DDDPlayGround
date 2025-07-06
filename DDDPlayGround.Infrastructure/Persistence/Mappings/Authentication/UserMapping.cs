using DDDPlayGround.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDPlayGround.Infrastructure.Persistence.Mappings.Authentication
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", SchemaName.Identity);

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Role)
                .IsRequired();

            builder.OwnsOne(u => u.Email, e =>
            {
                e.Property(p => p.Value)
                 .HasColumnName("Email")
                 .HasMaxLength(150);
            });

            builder.OwnsOne(u => u.FullName, e =>
            {
                e.Property(p=>p.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength (100); 
                
                e.Property(p=>p.LastName)
                .HasColumnName("LastName")
                .HasMaxLength (100);
            });

            builder.OwnsOne(u => u.PasswordHash, p =>
            {
                p.Property(ph => ph.Value)
                 .HasColumnName("PasswordHash")
                 .IsRequired();
            });

            builder.HasMany(u => u.Tokens)
                   .WithOne(t => t.User)
                   .HasForeignKey(t => t.UserId);
        }
    }

}
