using DDDPlayGround.Domain.Constants;
using DDDPlayGround.Domain.Entities.Authentication;
using DDDPlayGround.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace DDDPlayGround.Infrastructure.Persistence.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
            modelBuilder.HasDefaultSchema(SchemaName.DDD);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.AddInterceptors(new HandleAuditInterceptor());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
    }
}
