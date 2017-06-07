using Organizer.Infrastructure;
using System.Collections.Generic;
using System.Data;

namespace Organizer.DAL.Repository
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        private IDbContext _context;
        protected DataSet _dataSet;

        public RepositoryBase(IDbContext context, string baseSetName)
        {
            _context = context;
            _context.Set(baseSetName);
        }

        protected IDbContext Context => _context;

        #region Helpers

        protected ICollection<TEntity> ToList(IDbCommand command)
        {
            using (var reader = command.ExecuteReader())
            {
                var items = new List<TEntity>();
                while (reader.Read())
                {
                    var item = Map(reader);
                    items.Add(item);
                }
                return items;
            }
        }

        public abstract TEntity Map(IDataRecord record);

        public abstract TEntity Map(DataRow row);

        #endregion Helpers

        #region CRUD

        public abstract void Create(TEntity entity);

        public abstract void Delete(TEntity entity);

        public abstract void Delete(int id);

        public abstract TEntity Get(int id);

        public abstract TEntity Get(object key);

        public abstract ICollection<TEntity> GetAll();

        public abstract ICollection<TEntity> Select();

        public abstract void Update(TEntity entity);

        #endregion CRUD
    }
}