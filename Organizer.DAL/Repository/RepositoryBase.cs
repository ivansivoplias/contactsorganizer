﻿using Organizer.Common.Entities;
using Organizer.DAL.Helpers;
using Organizer.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Organizer.DAL.Repository
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        protected SqlConnection _connection;
        protected readonly IUnitOfWork _unitOfWork;

        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }
            _unitOfWork = unitOfWork;
            _connection = _unitOfWork.DataContext.Connection;
        }

        protected int Insert(TEntity entity, string insertSql, SqlTransaction sqlTransaction)
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

        protected int Update(TEntity entity, string updateSql, SqlTransaction sqlTransaction)
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

        protected int Delete(int id, string deleteSql, SqlTransaction sqlTransaction)
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

        protected TEntity GetById(int id, string getByIdSql)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = getByIdSql;
                cmd.CommandType = CommandType.Text;
                GetByIdCommandParameters(id, cmd);

                if (_unitOfWork.Transaction != null)
                {
                    cmd.Transaction = _unitOfWork.Transaction;
                }

                using (var reader = cmd.ExecuteReader())
                {
                    return Map(reader);
                }
            }
        }

        protected IEnumerable<TEntity> GetAll(string getAllSql)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = getAllSql;
                cmd.CommandType = CommandType.Text;
                if (_unitOfWork.Transaction != null)
                {
                    cmd.Transaction = _unitOfWork.Transaction;
                }

                using (var reader = cmd.ExecuteReader())
                {
                    return MapCollection(reader);
                }
            }
        }

        protected object GetValueOrDbNull(object value)
        {
            return value ?? DBNull.Value;
        }

        protected void TryParseEnum<TEnum>(SqlDataReader reader, string columnName, Action<TEnum> setter) where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var i = reader.GetOrdinal(columnName);

            TEnum result;

            if (!reader.IsDBNull(i) && Enum.TryParse(reader[columnName].ToString(), out result))
            {
                setter(result);
            }
        }

        protected void TryParseDateTime(SqlDataReader reader, string columnName, Action<DateTime> setter)
        {
            var i = reader.GetOrdinal(columnName);

            DateTime value;

            if (!reader.IsDBNull(i) && DateTime.TryParse(reader[columnName].ToString(), out value))
            {
                setter(value);
            }
        }

        protected abstract void InsertCommandParameters(TEntity entity, SqlCommand cmd);

        protected abstract void UpdateCommandParameters(TEntity entity, SqlCommand cmd);

        protected abstract void DeleteCommandParameters(int id, SqlCommand cmd);

        protected abstract void GetByIdCommandParameters(int id, SqlCommand cmd);

        protected abstract TEntity Map(SqlDataReader reader);

        protected abstract List<TEntity> MapCollection(SqlDataReader reader);

        public abstract int Insert(TEntity entity, SqlTransaction sqlTransaction);

        public abstract int Update(TEntity entity, SqlTransaction sqlTransaction);

        public abstract int Delete(int id, SqlTransaction sqlTransaction);

        public abstract TEntity GetById(int id);

        public abstract IEnumerable<TEntity> GetAll();

        public abstract int Count();

        protected int Count(string tableName)
        {
            var count = -1;
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = BaseQueries.GetCountQuery(tableName);
                cmd.CommandType = CommandType.Text;

                if (_unitOfWork.Transaction != null)
                {
                    cmd.Transaction = _unitOfWork.Transaction;
                }

                count = (int)cmd.ExecuteScalar();
            }
            return count;
        }

        public int FilteredCount(string filterSql, SqlParameter[] parameters)
        {
            var count = -1;
            using (var cmd = _connection.CreateCommand())
            {
                var filteredSql = BaseQueries.GetFilteredCountQuery(filterSql);

                QueryHelper.SetupCommand(cmd, filteredSql, parameters);

                if (_unitOfWork.Transaction != null)
                {
                    cmd.Transaction = _unitOfWork.Transaction;
                }

                count = (int)cmd.ExecuteScalar();
            }
            return count;
        }
    }
}