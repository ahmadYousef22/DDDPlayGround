using DDDPlayGround.Domain.Interfaces;
using DDDPlayGround.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DDDPlayGround.Infrastructure.Repositories
{
    public class UserRepository : Repository<User, Guid>, IUserRepository
    {
        private readonly DatabaseContext _dbContext;

        public UserRepository(DatabaseContext dbContext) : base(dbContext) { _dbContext = dbContext; }

        public async Task<User?> GetByUsername(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username && u.IsActive);
        }
    }
}
