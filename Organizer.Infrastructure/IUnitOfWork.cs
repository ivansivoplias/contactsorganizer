using System;

namespace Organizer.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : IEntity;

        void SaveChanges();
    }
}