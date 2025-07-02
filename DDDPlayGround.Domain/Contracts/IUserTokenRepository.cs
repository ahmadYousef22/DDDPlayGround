using DDDPlayGround.Domain.Entities.Authentication;

namespace DDDPlayGround.Domain.Interfaces
{
    public interface IUserTokenRepository 
    {
        Task AddAsync(UserToken token);
        Task<UserToken?> GetByTokenAsync(string token);
        Task<IEnumerable<UserToken>> GetActiveTokensByUserIdAsync(Guid userId);
        Task RevokeAllTokensAsync(Guid userId);
        Task RevokeAsync(UserToken token, string? revokedByIp = null, string? replacedByToken = null);
    }
}
