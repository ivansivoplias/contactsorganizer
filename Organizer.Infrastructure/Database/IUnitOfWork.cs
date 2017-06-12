using System.Data;

namespace Organizer.Infrastructure.Database
{
    public interface IUnitOfWork
    {
        IDbContext DataContext { get; }

        IDbTransaction BeginTransaction();

        void Commit();
    }
}