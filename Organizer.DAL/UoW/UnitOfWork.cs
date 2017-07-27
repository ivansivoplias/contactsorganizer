using Organizer.Common.Exceptions;
using Organizer.Infrastructure.Database;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Organizer.DAL.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDatabaseContextFactory _factory;
        private IDbContext _context;
        private SqlTransaction _transaction;

        public UnitOfWork(IDatabaseContextFactory factory)
        {
            _factory = factory;
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                try
                {
                    _transaction.Commit();
                }
                catch (Exception e)
                {
                    _transaction.Rollback();
                    throw new TransactionCommitFailedException(e);
                }
                _transaction.Dispose();
                _transaction = null;
            }
            else
            {
                throw new TransactionCommitFailedException();
            }
        }

        public IDbContext DataContext
        {
            get { return _context ?? (_context = _factory.MakeContext()); }
        }

        public void BeginTransaction()
        {
            if (_transaction != null)
            {
                throw new TransactionAlreadyExistsException();
            }

            if (DataContext.Connection != null && DataContext.Connection.State == ConnectionState.Open)
            {
                _transaction = DataContext.Connection.BeginTransaction();
            }
            else
            {
                throw new ConnnectionIsNullOrClosedException();
            }
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public SqlCommand CreateCommand()
        {
            SqlCommand result = DataContext.Connection.CreateCommand();

            if (_transaction != null)
                result.Transaction = _transaction;

            return result;
        }
    }
}