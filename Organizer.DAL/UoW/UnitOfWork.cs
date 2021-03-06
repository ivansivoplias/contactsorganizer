﻿using Organizer.Common.Exceptions;
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

        public SqlTransaction Transaction { get; private set; }

        public UnitOfWork(IDatabaseContextFactory factory)
        {
            _factory = factory;
        }

        public void Commit()
        {
            if (Transaction != null)
            {
                try
                {
                    Transaction.Commit();
                }
                catch (Exception e)
                {
                    Transaction.Rollback();
                    throw new TransactionCommitFailedException(e);
                }
                Transaction.Dispose();
                Transaction = null;
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

        public SqlTransaction BeginTransaction()
        {
            if (Transaction != null)
            {
                throw new TransactionAlreadyExistsException();
            }

            if (DataContext.Connection != null && DataContext.Connection.State == ConnectionState.Open)
            {
                Transaction = DataContext.Connection.BeginTransaction();
            }
            else
            {
                throw new ConnnectionIsNullOrClosedException();
            }
            return Transaction;
        }

        public void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
            }
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}