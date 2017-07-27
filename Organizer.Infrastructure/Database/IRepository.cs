using Organizer.Common.Entities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Organizer.Infrastructure.Database
{
    public interface IRepository<T> where T : IEntity
    {
        int Insert(T entity);

        int Update(T entity);

        int Delete(int id);

        T GetById(int id);

        IEnumerable<T> GetAll();

        int Count();

        int FilteredCount(string filterSql, SqlParameter[] parameters);
    }
}