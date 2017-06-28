using Organizer.Common.Entities;
using Organizer.DAL.Helpers;
using Organizer.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Organizer.DAL.Repository
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(IUnitOfWork uow) : base(uow)
        {
        }

        protected override void InsertCommandParameters(User entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@Login", entity.Login);
            cmd.Parameters.AddWithValue("@Password", entity.Password);
        }

        protected override void UpdateCommandParameters(User entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{UserQueries.UserId}", entity.Id);
            cmd.Parameters.AddWithValue("@Login", entity.Login);
            cmd.Parameters.AddWithValue("@Password", entity.Password);
        }

        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{UserQueries.UserId}", id);
        }

        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{UserQueries.UserId}", id);
        }

        protected override User Map(SqlDataReader reader)
        {
            User user = null;
            if (reader.HasRows)
            {
                user = new User();
                while (reader.Read())
                {
                    user.Id = Convert.ToInt32(reader[UserQueries.UserId].ToString());
                    user.Login = reader["Login"].ToString();
                    user.Password = reader["Password"].ToString();
                }
            }
            return user;
        }

        protected override List<User> MapCollection(SqlDataReader reader)
        {
            List<User> users = new List<User>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var person = new User();
                    person.Id = Convert.ToInt32(reader[UserQueries.UserId].ToString());
                    person.Login = reader["Login"].ToString();
                    person.Password = reader["Password"].ToString();
                    users.Add(person);
                }
            }
            return users;
        }

        public override int Insert(User entity, SqlTransaction sqlTransaction)
        {
            var query = UserQueries.GetInsertQuery();
            return Insert(entity, query, sqlTransaction);
        }

        public override int Update(User entity, SqlTransaction sqlTransaction)
        {
            var query = UserQueries.GetUpdateQuery();
            return Update(entity, query, sqlTransaction);
        }

        public override int Delete(int id, SqlTransaction sqlTransaction)
        {
            var query = UserQueries.GetDeleteQuery();
            return Delete(id, query, sqlTransaction);
        }

        public override User GetById(int id)
        {
            var query = UserQueries.GetGetByIdQuery();

            return GetById(id, query);
        }

        public override IEnumerable<User> GetAll()
        {
            return GetAll(UserQueries.GetAllQuery());
        }

        public User FindByLogin(string login)
        {
            User result = null;
            var query = UserQueries.FindByLoginQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@Login", login));
                using (var reader = cmd.ExecuteReader())
                {
                    result = Map(reader);
                }
            }

            return result;
        }

        public override int Count()
        {
            return Count(UserQueries.UserTable);
        }
    }
}