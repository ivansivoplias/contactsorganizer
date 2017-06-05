using Organizer.DAL.Repository;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace Organizer.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbTransaction _transaction;
        private readonly Action<IUnitOfWork> _rolledBack;
        private readonly Action<IUnitOfWork> _committed;
        private IRepositoryProvider _repositoryProvider;

        public UnitOfWork(IDbTransaction transaction, Action<IUnitOfWork> rolledBack, Action<IUnitOfWork> committed)
        {
            Transaction = transaction;
            _rolledBack = rolledBack;
            _committed = committed;
            _repositoryProvider = new RepositoryProvider();
        }

        public IDbTransaction Transaction
        {
            get { return _transaction; }
            private set { _transaction = value; }
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : IEntity
        {
            var repositoryType = typeof(IRepository<TEntity>);
            var repositoryName = repositoryType.Name;

            return _repositoryProvider.GetRepositoryForKey<TEntity>(repositoryName);
        }

        public void SaveChanges()
        {
            if (_transaction == null)
                throw new InvalidOperationException("May not call save changes twice.");

            _transaction.Commit();
            _committed(this);
            _transaction = null;
        }

        public void Dispose()
        {
            if (_transaction == null)
                return;

            _transaction.Rollback();
            _transaction.Dispose();
            _rolledBack(this);
            _transaction = null;
        }
    }
}