using System;
using System.Data.SqlClient;

namespace Organizer.Infrastructure.Database
{
    public interface IUnitOfWork : IDisposable
    {
        IDbContext DataContext { get; }

        SqlTransaction Transaction { get; }

        SqlTransaction BeginTransaction();

        void Commit();
    }
}