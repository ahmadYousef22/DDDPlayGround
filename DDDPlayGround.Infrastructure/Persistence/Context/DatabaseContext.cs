using DDDPlayGround.Domain.Entities.Authentication;
using DDDPlayGround.Domain.Entities.Common;
using DDDPlayGround.Infrastructure.Persistence.Interceptors;
using DDDPlayGround.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace DDDPlayGround.Infrastructure.Persistence.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
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

        public DbSet<User> Users => Set<User>();
        public DbSet<UserToken> UserTokens => Set<UserToken>();
        public DbSet<LogEntry> LogEntry => Set<LogEntry>();
    }
}
