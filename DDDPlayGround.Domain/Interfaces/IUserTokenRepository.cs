using DDDPlayGround.Domain.Entities.Authentication;

namespace DDDPlayGround.Domain.Interfaces
{
    public interface IUserTokenRepository
    {
        Task<UserToken?> GetByTokenAsync(string token); 
        Task AddAsync(UserToken token);                  
        Task UpdateAsync(UserToken token);                
        Task DeleteAsync(UserToken token);               
    }
}
