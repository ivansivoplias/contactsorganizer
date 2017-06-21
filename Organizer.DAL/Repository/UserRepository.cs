using Organizer.Common.Entities;
using Organizer.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Organizer.DAL.Repository
{
    public class UserRepository : RepositoryBase<User>
    {
        private readonly string _userId;
        private readonly string _userTable;

        public UserRepository(IUnitOfWork uow) : base(uow)
        {
            var user = new User();
            _userId = user.IdColumnName;
            _userTable = user.TableName;
        }

        protected override void InsertCommandParameters(User entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@Login", entity.Login);
            cmd.Parameters.AddWithValue("@Password", entity.Password);
        }

        protected override void UpdateCommandParameters(User entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{entity.IdColumnName}", entity.Id);
            cmd.Parameters.AddWithValue("@Login", entity.Login);
            cmd.Parameters.AddWithValue("@Password", entity.Password);
        }

        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new User().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new User().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        protected override User Map(SqlDataReader reader)
        {
            var user = new User();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    user.Id = Convert.ToInt32(reader[user.IdColumnName].ToString());
                    user.Login = reader["Login"].ToString();
                    user.Password = reader["Password"].ToString();
                }
            }
            return user;
        }

        protected override List<User> MapCollection(SqlDataReader reader)
        {
            var users = new List<User>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var person = new User();
                    person.Id = Convert.ToInt32(reader[person.IdColumnName].ToString());
                    person.Login = reader["Login"].ToString();
                    person.Password = reader["Password"].ToString();
                    users.Add(person);
                }
            }
            return users;
        }

        public override int Insert(User entity, SqlTransaction sqlTransaction)
        {
            var query = $"INSERT INTO {_userTable} (Login, Password)" +
                " VALUES(@Login, @Password)";
            return Insert(entity, query, sqlTransaction);
        }

        public override int Update(User entity, SqlTransaction sqlTransaction)
        {
            var query = $"UPDATE {_userTable} SET Login = @Login," +
                $" Password = @Password WHERE {_userId} = @{_userId}";
            return Update(entity, query, sqlTransaction);
        }

        public override int Delete(int id, SqlTransaction sqlTransaction)
        {
            var query = $"DELETE FROM {_userTable} WHERE {_userId} = @{_userId}";
            return Delete(id, query, sqlTransaction);
        }

        public override User GetById(int id)
        {
            var query = $"SELECT TOP 1 * FROM {_userTable} WHERE {_userId} = @{_userId}";

            return GetById(id, query);
        }

        public override IEnumerable<User> GetAll()
        {
            return GetAll($"SELECT * FROM {_userTable}");
        }
    }
}