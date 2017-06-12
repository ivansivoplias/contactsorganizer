using System.Collections.Generic;
using System.Data;

namespace Organizer.Infrastructure.Database
{
    public interface IRepository<T> where T : IEntity
    {
        int Insert(T entity, string insertSql, IDbTransaction sqlTransaction);

        int Update(T entity, string updateSql, IDbTransaction sqlTransaction);

        int Delete(int id, string deleteSql, IDbTransaction sqlTransaction);

        T GetById(int id, string getByIdSql);

        IEnumerable<T> GetAll(string getAllSql);
    }
}