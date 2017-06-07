namespace Organizer.Infrastructure
{
    public interface IRepositoryProvider
    {
        IRepository<TEntity> GetRepositoryForKey<TEntity>(string typeKey, IDbContext context) where TEntity : IEntity;
    }
}