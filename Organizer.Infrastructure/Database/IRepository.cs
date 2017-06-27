using Organizer.Common.Entities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Organizer.Infrastructure.Database
{
    public interface IRepository<T> where T : IEntity
    {
        int Insert(T entity, SqlTransaction sqlTransaction);

        int Update(T entity, SqlTransaction sqlTransaction);

        int Delete(int id, SqlTransaction sqlTransaction);

        T GetById(int id);

        IEnumerable<T> GetAll();

        int Count();

        int FilteredCount(string filterSql, SqlParameter[] parameters);
    }
}