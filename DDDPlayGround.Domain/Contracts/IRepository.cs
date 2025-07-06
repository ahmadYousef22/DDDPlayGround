namespace DDDPlayGround.Domain.Contracts
{
    public interface IRepository<TEntity, TKey> where TEntity : class, IAggregateRoot
    {
        Task<TEntity?> GetById(TKey id);
        Task<IEnumerable<TEntity>> GetAll();
        Task Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<bool> Exists(TKey id);
    }
}
