using Organizer.Common.Entities;
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

        /// <summary>
        /// Passes the parameters for Insert Statement
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cmd"></param>
        protected override void InsertCommandParameters(User entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@Login", entity.Login);
            cmd.Parameters.AddWithValue("@Password", entity.Password);
        }

        /// <summary>
        /// Passes the parameters for Update Statement
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cmd"></param>
        protected override void UpdateCommandParameters(User entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{entity.IdColumnName}", entity.Id);
            cmd.Parameters.AddWithValue("@Login", entity.Login);
            cmd.Parameters.AddWithValue("@Password", entity.Password);
        }

        /// <summary>
        /// Passes the parameters to command for Delete Statement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cmd"></param>
        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new User().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        /// <summary>
        /// Passes the parameters to command for populate by key statement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cmd"></param>
        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new User().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        /// <summary>
        /// Maps data for populate by key statement
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Maps data for populate all statement
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
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
    }
}