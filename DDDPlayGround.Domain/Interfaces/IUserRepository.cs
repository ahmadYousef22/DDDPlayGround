
namespace DDDPlayGround.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> GetByUsername(string username);  
    }
}
