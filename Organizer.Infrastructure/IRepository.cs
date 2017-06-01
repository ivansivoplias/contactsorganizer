using System.Collections.Generic;

namespace Organizer.Infrastructure
{
    public interface IRepository<TEntity>
    {
        TEntity Get(int id);

        TEntity Get(object key);

        ICollection<TEntity> GetAll();

        ICollection<TEntity> Select();

        #region CRUD operations

        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(int id);

        #endregion CRUD operations
    }
}