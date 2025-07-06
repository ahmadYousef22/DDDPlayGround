namespace DDDPlayGround.Domain.Contracts
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> GetByUsername(string username);
    }
}
