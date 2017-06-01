using Organizer.DAL.Repository;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private Dictionary<string, object> _repositories;
        private IRepositoryProvider _repositoryProvider;

        public UnitOfWork()
        {
            _repositoryProvider = new RepositoryProvider();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : IEntity
        {
            IRepository<TEntity> result = null;

            if (_repositories == null)
            {
                _repositories = new Dictionary<string, object>();
            }

            var repositoryType = typeof(IRepository<TEntity>);
            var repositoryName = repositoryType.Name;

            if (_repositories.ContainsKey(repositoryName))
            {
                result = (IRepository<TEntity>)_repositories[repositoryName];
            }
            else
            {
                var repository = _repositoryProvider.GetRepositoryForKey<TEntity>(repositoryName);
                _repositories[repositoryName] = repository;
            }

            return result;
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}