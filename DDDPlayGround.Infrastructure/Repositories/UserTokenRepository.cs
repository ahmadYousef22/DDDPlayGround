using DDDPlayGround.Domain.Contracts;
using DDDPlayGround.Domain.Entities.Authentication;
using DDDPlayGround.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DDDPlayGround.Infrastructure.Repositories
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly DatabaseContext _context;

        public UserTokenRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<UserToken?> GetByTokenAsync(string token)
        {
            return await _context.UserTokens
                .Include(ut => ut.User)
                .FirstOrDefaultAsync(ut => ut.Token == token);
        }

        public async Task AddAsync(UserToken token)
        {
            await _context.UserTokens.AddAsync(token);
        }

        public async Task RevokeAsync(UserToken token, string? revokedByIp = null, string? replacedByToken = null)
        {
            token.Revoke(revokedByIp, replacedByToken);
            _context.UserTokens.Update(token);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<UserToken>> GetActiveTokensByUserIdAsync(Guid userId)
        {
            return await _context.UserTokens
                .Where(ut => ut.UserId == userId && ut.IsActive)
                .ToListAsync();
        }

        public async Task RevokeAllTokensAsync(Guid userId)
        {
            var activeTokens = await _context.UserTokens
                .Where(t => t.UserId == userId && t.IsActive)
                .ToListAsync();

            foreach (var token in activeTokens)
            {
                token.Revoke("System", null);
            }

            _context.UserTokens.UpdateRange(activeTokens);
        }
    }
}
