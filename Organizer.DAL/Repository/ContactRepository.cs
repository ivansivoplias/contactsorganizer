using System;
using System.Collections.Generic;
using Organizer.Infrastructure.Database;
using System.Data.SqlClient;
using Organizer.Common.Entities;

namespace Organizer.DAL.Repository
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        private readonly string _contactTable;
        private readonly string _contactId;

        public ContactRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            var cont = new Contact();
            _contactId = cont.IdColumnName;
            _contactTable = cont.TableName;
        }

        protected override void InsertCommandParameters(Contact entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@PrimaryPhone", entity.PrimaryPhone);
            cmd.Parameters.AddWithValue("@UserId", entity.UserId);
        }

        protected override void UpdateCommandParameters(Contact entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{entity.IdColumnName}", entity.Id);
            cmd.Parameters.AddWithValue("@PrimaryPhone", entity.PrimaryPhone);
            cmd.Parameters.AddWithValue("@UserId", entity.UserId);
        }

        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{_contactId}", id);
        }

        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{_contactId}", id);
        }

        protected override Contact Map(SqlDataReader reader)
        {
            var contact = new Contact();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    contact.Id = Convert.ToInt32(reader[_contactId].ToString());
                    contact.PrimaryPhone = reader["PrimaryPhone"].ToString();
                    contact.UserId = Convert.ToInt32(reader["UserId"].ToString());
                }
            }
            return contact;
        }

        protected override List<Contact> MapCollection(SqlDataReader reader)
        {
            var contacts = new List<Contact>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var contact = new Contact();
                    contact.Id = Convert.ToInt32(reader[_contactId].ToString());
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

            var socialInfoTable = socialInfo.TableName;
            string query = $"SELECT * FROM {_contactTable} " +
                $"INNER JOIN {socialInfoTable} ON {_contactTable}.{_contactId} = {socialInfoTable}.ContactId "
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

            var personalInfoTable = new PersonalInfo().TableName;
            var personalInfoId = new PersonalInfo().IdColumnName;
            string query = $"SELECT * FROM {_contactTable} " +
                $"INNER JOIN {personalInfoTable} ON {_contactTable}.{_contactId} = {personalInfoTable}.{personalInfoId}"
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

            var personalInfoTable = new PersonalInfo().TableName;
            var personalInfoId = new PersonalInfo().IdColumnName;
            string query = $"SELECT * FROM {_contactTable} " +
                $"INNER JOIN {personalInfoTable} ON {_contactTable}.{_contactId} = {personalInfoTable}.{personalInfoId}"
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

            var personalInfoTable = new PersonalInfo().TableName;
            var personalInfoId = new PersonalInfo().IdColumnName;
            string query = $"SELECT * FROM {_contactTable} " +
                $"INNER JOIN {personalInfoTable} ON {_contactTable}.{_contactId} = {personalInfoTable}.{personalInfoId}"
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

            var personalInfoTable = info.TableName;
            var personalInfoId = info.IdColumnName;
            string query = $"SELECT * FROM {_contactTable} " +
                $"INNER JOIN {personalInfoTable} ON {_contactTable}.{_contactId} = {personalInfoTable}.{personalInfoId}"
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

            var personalInfoTable = new PersonalInfo().TableName;
            var personalInfoId = new PersonalInfo().IdColumnName;
            string query = $"SELECT TOP 1 * FROM {_contactTable} " +
                $"INNER JOIN {personalInfoTable} ON {_contactTable}.{_contactId} = {personalInfoTable}.{personalInfoId}"
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

            string query = $"SELECT TOP 1 * FROM {_contactTable}" +
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

            var personalInfoTable = new PersonalInfo().TableName;
            var personalInfoId = new PersonalInfo().IdColumnName;
            string query = $"SELECT * FROM {_contactTable} " +
                $"INNER JOIN {personalInfoTable} ON {_contactTable}.{_contactId} = {personalInfoTable}.{personalInfoId}"
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

        public override int Insert(Contact entity, SqlTransaction sqlTransaction)
        {
            var query = $"INSERT INTO {_contactTable} (PrimaryPhone, UserId)" +
                " VALUES(@PrimaryPhone, @UserId)";
            return Insert(entity, query, sqlTransaction);
        }

        public override int Update(Contact entity, SqlTransaction sqlTransaction)
        {
            var query = $"UPDATE {_contactTable} SET PrimaryPhone = @PrimaryPhone," +
                $" UserId = @UserId, WHERE {_contactId} = @{_contactId}";
            return Update(entity, query, sqlTransaction);
        }

        public override int Delete(int id, SqlTransaction sqlTransaction)
        {
            var query = $"DELETE FROM {_contactTable} WHERE {_contactId} = @{_contactId}";
            return Delete(id, query, sqlTransaction);
        }

        public override Contact GetById(int id)
        {
            var query = $"SELECT TOP 1 * FROM {_contactTable} WHERE {_contactId} = @{_contactId}";

            return GetById(id, query);
        }

        public override IEnumerable<Contact> GetAll()
        {
            return GetAll($"SELECT * FROM {_contactTable}");
        }
    }
}