using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Organizer.Common.Entities;

namespace Organizer.Infrastructure.Database
{
    public interface IRepository<T> where T : IEntity
    {
        int Insert(T entity, string insertSql, SqlTransaction sqlTransaction);

        int Update(T entity, string updateSql, SqlTransaction sqlTransaction);

        int Delete(int id, string deleteSql, SqlTransaction sqlTransaction);

        T GetById(int id, string getByIdSql);

        IEnumerable<T> GetAll(string getAllSql);
    }
}