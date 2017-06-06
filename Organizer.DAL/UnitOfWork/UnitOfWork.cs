using Organizer.DAL.Repository;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace Organizer.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbTransaction _transaction;
        private IDbContext _context;
        private readonly Action _rolledBack;
        private readonly Action _committed;
        private readonly IRepositoryProvider _repositoryProvider;

        public UnitOfWork(IDbContext context, Action rolledBack, Action committed)
        {
            Transaction = context.CurrentTransaction;
            _context = context;
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

            return _repositoryProvider.GetRepositoryForKey<TEntity>(repositoryName, _context);
        }

        public void SaveChanges()
        {
            if (_transaction == null)
                throw new InvalidOperationException("May not call save changes twice.");

            _transaction.Commit();
            _committed();
            _transaction = null;
        }

        public void Dispose()
        {
            if (_transaction == null)
                return;

            _transaction.Rollback();
            _transaction.Dispose();
            _rolledBack();
            _transaction = null;
        }
    }
}