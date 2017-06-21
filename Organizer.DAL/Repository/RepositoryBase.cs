using Organizer.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Organizer.Common.Entities;

namespace Organizer.DAL.Repository
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        protected SqlConnection _connection;
        protected readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initialize the connection
        /// </summary>
        /// <param name="unitOfWork">UnitOfWork</param>
        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }
            _unitOfWork = unitOfWork;
            _connection = _unitOfWork.DataContext.Connection;
        }

        /// <summary>
        /// Base Method for Insert Data
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="insertSql"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public int Insert(TEntity entity, string insertSql, SqlTransaction sqlTransaction)
        {
            int i = 0;
            try
            {
                using (var cmd = _connection.CreateCommand())
                {
                    cmd.CommandText = insertSql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Transaction = sqlTransaction;
                    InsertCommandParameters(entity, cmd);
                    i = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return i;
        }

        /// <summary>
        /// Base Method for Update Data
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="updateSql"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public int Update(TEntity entity, string updateSql, SqlTransaction sqlTransaction)
        {
            int i = 0;
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = updateSql;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = sqlTransaction;
                UpdateCommandParameters(entity, cmd);
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }

        /// <summary>
        /// Base Method for Delete Data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deleteSql"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public int Delete(int id, string deleteSql, SqlTransaction sqlTransaction)
        {
            int i = 0;
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = deleteSql;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = sqlTransaction;
                DeleteCommandParameters(id, cmd);
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }

        /// <summary>
        /// Base Method for Populate Data by key
        /// </summary>
        /// <param name="id"></param>
        /// <param name="getByIdSql"></param>
        /// <returns></returns>
        public TEntity GetById(int id, string getByIdSql)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = getByIdSql;
                cmd.CommandType = CommandType.Text;
                GetByIdCommandParameters(id, cmd);
                using (var reader = cmd.ExecuteReader())
                {
                    return Map(reader);
                }
            }
        }

        /// <summary>
        /// Base Method for Populate All Data
        /// </summary>
        /// <param name="getAllSql"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(string getAllSql)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = getAllSql;
                cmd.CommandType = CommandType.Text;
                using (var reader = cmd.ExecuteReader())
                {
                    return MapCollection(reader);
                }
            }
        }

        protected abstract void InsertCommandParameters(TEntity entity, SqlCommand cmd);

        protected abstract void UpdateCommandParameters(TEntity entity, SqlCommand cmd);

        protected abstract void DeleteCommandParameters(int id, SqlCommand cmd);

        protected abstract void GetByIdCommandParameters(int id, SqlCommand cmd);

        protected abstract TEntity Map(SqlDataReader reader);

        protected abstract List<TEntity> MapCollection(SqlDataReader reader);
    }
}