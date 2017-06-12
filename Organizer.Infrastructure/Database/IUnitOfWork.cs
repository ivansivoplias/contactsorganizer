using System.Data.SqlClient;

namespace Organizer.Infrastructure.Database
{
    public interface IUnitOfWork
    {
        IDbContext DataContext { get; }

        SqlTransaction BeginTransaction();

        void Commit();
    }
}