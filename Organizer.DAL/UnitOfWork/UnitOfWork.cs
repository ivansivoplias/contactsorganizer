using Organizer.DAL.Repository;
using Organizer.Infrastructure;

namespace Organizer.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;

        private readonly IRepositoryProvider _repositoryProvider;

        public UnitOfWork(IDbContext context)
        {
            _context = context;
            _repositoryProvider = new RepositoryProvider();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : IEntity
        {
            var repositoryType = typeof(IRepository<TEntity>);
            var repositoryName = repositoryType.Name;

            return _repositoryProvider.GetRepositoryForKey<TEntity>(repositoryName, _context);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}