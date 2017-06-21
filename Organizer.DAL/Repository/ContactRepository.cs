using System;
using System.Collections.Generic;
using Organizer.Infrastructure.Database;
using System.Data.SqlClient;
using Organizer.Common.Entities;
using Organizer.DAL.Abstract;

namespace Organizer.DAL.Repository
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Passes the parameters for Insert Statement
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cmd"></param>
        protected override void InsertCommandParameters(Contact entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@PrimaryPhone", entity.PrimaryPhone);
            cmd.Parameters.AddWithValue("@UserId", entity.UserId);
        }

        /// <summary>
        /// Passes the parameters for Update Statement
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cmd"></param>
        protected override void UpdateCommandParameters(Contact entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{entity.IdColumnName}", entity.Id);
            cmd.Parameters.AddWithValue("@PrimaryPhone", entity.PrimaryPhone);
            cmd.Parameters.AddWithValue("@UserId", entity.UserId);
        }

        /// <summary>
        /// Passes the parameters to command for Delete Statement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cmd"></param>
        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new Contact().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        /// <summary>
        /// Passes the parameters to command for populate by key statement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cmd"></param>
        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new Contact().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        /// <summary>
        /// Maps data for populate by key statement
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override Contact Map(SqlDataReader reader)
        {
            var contact = new Contact();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    contact.Id = Convert.ToInt32(reader[contact.IdColumnName].ToString());
                    contact.PrimaryPhone = reader["PrimaryPhone"].ToString();
                    contact.UserId = Convert.ToInt32(reader["UserId"].ToString());
                }
            }
            return contact;
        }

        /// <summary>
        /// Maps data for populate all statement
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override List<Contact> MapCollection(SqlDataReader reader)
        {
            var contacts = new List<Contact>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var contact = new Contact();
                    contact.Id = Convert.ToInt32(reader[contact.IdColumnName].ToString());
                    contact.PrimaryPhone = reader["PrimaryPhone"].ToString();
                    contact.UserId = Convert.ToInt32(reader["UserId"].ToString());
                    contacts.Add(contact);
                }
            }
            return contacts;
        }

        public IEnumerable<Contact> FilterBySocialInfo(int userId, SocialInfo socialInfo)
        {
            IEnumerable<Contact> result = null;

            var contactTable = new Contact().TableName;
            var contactId = new Contact().IdColumnName;
            var socialInfoTable = socialInfo.TableName;
            string query = $"SELECT * FROM {contactTable} " +
                $"INNER JOIN {socialInfoTable} ON {contactTable}.{contactId} = {socialInfoTable}.ContactId "
                + "WHERE UserId = @UserId AND AppName = @AppName AND AppId = @AppId";

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ContactId", socialInfo.ContactId);
                cmd.Parameters.AddWithValue("@AppId", socialInfo.AppId);
                cmd.Parameters.AddWithValue("@AppName", socialInfo.AppName);

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Contact> FilterByFirstName(int userId, string firstName)
        {
            IEnumerable<Contact> result = null;

            var contactTable = new Contact().TableName;
            var contactId = new Contact().IdColumnName;
            var personalInfoTable = new PersonalInfo().TableName;
            var personalInfoId = new PersonalInfo().IdColumnName;
            string query = $"SELECT * FROM {contactTable} " +
                $"INNER JOIN {personalInfoTable} ON {contactTable}.{contactId} = {personalInfoTable}.{personalInfoId}"
                + " WHERE UserId = @UserId AND FirstName = @FirstName";

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@FirstName", firstName);

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Contact> FilterByLastName(int userId, string lastName)
        {
            IEnumerable<Contact> result = null;

            var contactTable = new Contact().TableName;
            var contactId = new Contact().IdColumnName;
            var personalInfoTable = new PersonalInfo().TableName;
            var personalInfoId = new PersonalInfo().IdColumnName;
            string query = $"SELECT * FROM {contactTable} " +
                $"INNER JOIN {personalInfoTable} ON {contactTable}.{contactId} = {personalInfoTable}.{personalInfoId}"
                + " WHERE UserId = @UserId AND Lastname = @LastName";

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@LastName", lastName);

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Contact> FilterByMiddleName(int userId, string middleName)
        {
            IEnumerable<Contact> result = null;

            var contactTable = new Contact().TableName;
            var contactId = new Contact().IdColumnName;
            var personalInfoTable = new PersonalInfo().TableName;
            var personalInfoId = new PersonalInfo().IdColumnName;
            string query = $"SELECT * FROM {contactTable} " +
                $"INNER JOIN {personalInfoTable} ON {contactTable}.{contactId} = {personalInfoTable}.{personalInfoId}"
                + " WHERE UserId = @UserId AND MiddleName = @MiddleName";

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@MiddleName", middleName);

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Contact> FilterByPersonalInfo(int userId, PersonalInfo info)
        {
            IEnumerable<Contact> result = null;

            var contactTable = new Contact().TableName;
            var contactId = new Contact().IdColumnName;
            var personalInfoTable = info.TableName;
            var personalInfoId = info.IdColumnName;
            string query = $"SELECT * FROM {contactTable} " +
                $"INNER JOIN {personalInfoTable} ON {contactTable}.{contactId} = {personalInfoTable}.{personalInfoId}"
                + " WHERE UserId = @UserId AND FirstName = @FirstName" +
                " AND Lastname = @LastName AND" +
                " MiddleName = @MiddleName AND Nickname = @NickName AND Email = @Email";

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@FirstName", info.FirstName);
                cmd.Parameters.AddWithValue("@LastName", info.Lastname);
                cmd.Parameters.AddWithValue("@MiddleName", info.MiddleName);
                cmd.Parameters.AddWithValue("@NickName", info.Nickname);
                cmd.Parameters.AddWithValue("@Email", info.Email);

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public Contact FindByNickName(int userId, string nickname)
        {
            Contact result = null;

            var contactTable = new Contact().TableName;
            var contactId = new Contact().IdColumnName;
            var personalInfoTable = new PersonalInfo().TableName;
            var personalInfoId = new PersonalInfo().IdColumnName;
            string query = $"SELECT TOP 1 * FROM {contactTable} " +
                $"INNER JOIN {personalInfoTable} ON {contactTable}.{contactId} = {personalInfoTable}.{personalInfoId}"
                + " WHERE UserId = @UserId AND Nickname = @NickName";

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@NickName", nickname);

                using (var reader = cmd.ExecuteReader())
                {
                    result = Map(reader);
                }
            }

            return result;
        }

        public Contact FindByPrimaryPhone(int userId, string phone)
        {
            Contact result = null;

            var contactTable = new Contact().TableName;
            string query = $"SELECT TOP 1 * FROM {contactTable}" +
                " WHERE UserId = @UserId AND PrimaryPhone = @Phone";

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Phone", phone);

                using (var reader = cmd.ExecuteReader())
                {
                    result = Map(reader);
                }
            }

            return result;
        }

        public IEnumerable<Contact> FilterByEmail(int userId, string email)
        {
            IEnumerable<Contact> result = null;

            var contactTable = new Contact().TableName;
            var contactId = new Contact().IdColumnName;
            var personalInfoTable = new PersonalInfo().TableName;
            var personalInfoId = new PersonalInfo().IdColumnName;
            string query = $"SELECT * FROM {contactTable} " +
                $"INNER JOIN {personalInfoTable} ON {contactTable}.{contactId} = {personalInfoTable}.{personalInfoId}"
                + " WHERE UserId = @UserId AND Email = @Email";

            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Email", email);

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }
    }
}