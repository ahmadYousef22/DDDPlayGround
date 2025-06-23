using DDDPlayGround.Domain.Entities.Authentication;
using DDDPlayGround.Domain.Interfaces;
using DDDPlayGround.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DDDPlayGround.Infrastructure.Repositories
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly DatabaseContext _dbContext;
        public UserTokenRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserToken?> GetByTokenAsync(string token)
        {
            return await _dbContext.UserTokens
                .Include(ut => ut.User)
                .FirstOrDefaultAsync(ut => ut.Token == token && !ut.IsRevoked && !ut.IsExpired);
        }

        public async Task AddAsync(UserToken token)
        {
            await _dbContext.UserTokens.AddAsync(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserToken token)
        {
            _dbContext.UserTokens.Update(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserToken token)
        {
            _dbContext.UserTokens.Remove(token);
            await _dbContext.SaveChangesAsync();
        }
    }
}
