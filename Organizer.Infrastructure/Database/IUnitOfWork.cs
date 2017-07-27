using System;
using System.Data.SqlClient;

namespace Organizer.Infrastructure.Database
{
    public interface IUnitOfWork : IDisposable
    {
        IDbContext DataContext { get; }

        void BeginTransaction();

        SqlCommand CreateCommand();

        void Commit();
    }
}