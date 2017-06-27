using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Organizer.Common.Entities;

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

        int FilteredCount(string filterSql);
    }
}