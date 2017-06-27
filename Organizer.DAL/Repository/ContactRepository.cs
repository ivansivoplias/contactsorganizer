using System;
using System.Collections.Generic;
using Organizer.Infrastructure.Database;
using System.Data.SqlClient;
using Organizer.Common.Entities;
using Organizer.DAL.Helpers;

namespace Organizer.DAL.Repository
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override void InsertCommandParameters(Contact entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@PrimaryPhone", entity.PrimaryPhone);
            cmd.Parameters.AddWithValue("@UserId", entity.UserId);
        }

        protected override void UpdateCommandParameters(Contact entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{ContactQueries.ContactId}", entity.Id);
            cmd.Parameters.AddWithValue("@PrimaryPhone", entity.PrimaryPhone);
        }

        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{ContactQueries.ContactId}", id);
        }

        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{ContactQueries.ContactId}", id);
        }

        protected override Contact Map(SqlDataReader reader)
        {
            Contact contact = null;
            if (reader.HasRows)
            {
                contact = new Contact();

                while (reader.Read())
                {
                    contact.Id = Convert.ToInt32(reader[ContactQueries.ContactId].ToString());
                    contact.PrimaryPhone = reader["PrimaryPhone"].ToString();
                    contact.UserId = Convert.ToInt32(reader["UserId"].ToString());
                }
            }
            return contact;
        }

        protected override List<Contact> MapCollection(SqlDataReader reader)
        {
            List<Contact> contacts = null;
            if (reader.HasRows)
            {
                contacts = new List<Contact>();
                while (reader.Read())
                {
                    var contact = new Contact();
                    contact.Id = Convert.ToInt32(reader[ContactQueries.ContactId].ToString());
                    contact.PrimaryPhone = reader["PrimaryPhone"].ToString();
                    contact.UserId = Convert.ToInt32(reader["UserId"].ToString());
                    contacts.Add(contact);
                }
            }
            return contacts;
        }

        public IEnumerable<Contact> FilterBySocialInfoAppIdLike(int userId, SocialInfo socialInfo, int? pageSize = null, int? page = null)
        {
            IEnumerable<Contact> result = null;
            string query = ContactQueries.GetFilterBySocialInfoQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("AppName", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId),
                    new SqlParameter("@AppId", socialInfo.AppId.MakeLikeExpression()),
                    new SqlParameter("@AppName", socialInfo.AppName));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Contact> FilterByFirstNameStartsWith(int userId, string firstName, int? pageSize = null, int? page = null)
        {
            IEnumerable<Contact> result = null;

            string query = ContactQueries.GetFilterByFirstNameQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("FirstName", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId),
                    new SqlParameter("@FirstName", firstName.MakeStartsWithLikeExpression()));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Contact> FilterByLastNameStartsWith(int userId, string lastName, int? pageSize = null, int? page = null)
        {
            IEnumerable<Contact> result = null;
            string query = ContactQueries.GetFilterByLastNameQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("Lastname", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, ContactParams.GetFilterByLastnameParams(userId, lastName));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Contact> FilterByMiddleNameStartsWith(int userId, string middleName, int? pageSize = null, int? page = null)
        {
            IEnumerable<Contact> result = null;
            string query = ContactQueries.GetFilterByMiddleNameQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("MiddleName", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, ContactParams.GetFilterByMiddleNameParams(userId, middleName));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Contact> FilterByPersonalInfo(int userId, PersonalInfo info, int? pageSize = null, int? page = null)
        {
            IEnumerable<Contact> result = null;
            string query = ContactQueries.GetFilterByPersonalInfoQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("FirstName", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, ContactParams.GetFilterByPersonalInfoParams(userId, info));

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
            string query = ContactQueries.GetFindByNicknameQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId),
                    new SqlParameter("@NickName", nickname));

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

            string query = ContactQueries.GetFindByPrimaryPhoneQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId),
                    new SqlParameter("@Phone", phone));

                using (var reader = cmd.ExecuteReader())
                {
                    result = Map(reader);
                }
            }

            return result;
        }

        public IEnumerable<Contact> FilterByEmailStartsWith(int userId, string email, int? pageSize = null, int? page = null)
        {
            IEnumerable<Contact> result = null;
            string query = ContactQueries.GetFilterByEmailQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("Email", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, ContactParams.GetFilterByEmailParams(userId, email));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public override int Insert(Contact entity, SqlTransaction sqlTransaction)
        {
            var query = ContactQueries.GetInsertQuery();
            return Insert(entity, query, sqlTransaction);
        }

        public override int Update(Contact entity, SqlTransaction sqlTransaction)
        {
            var query = ContactQueries.GetUpdateQuery();
            return Update(entity, query, sqlTransaction);
        }

        public override int Delete(int id, SqlTransaction sqlTransaction)
        {
            var query = ContactQueries.GetDeleteQuery();
            return Delete(id, query, sqlTransaction);
        }

        public override Contact GetById(int id)
        {
            var query = ContactQueries.GetGetByIdQuery();

            return GetById(id, query);
        }

        public IEnumerable<Contact> GetUserContacts(int userId, int? pageSize = null, int? page = null)
        {
            IEnumerable<Contact> result = null;
            string query = ContactQueries.GetUserContactsQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging(ContactQueries.ContactId, pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, ContactParams.GetGetUserContactsParams(userId));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public override IEnumerable<Contact> GetAll()
        {
            return GetAll(ContactQueries.GetGetAllQuery());
        }

        public override int Count()
        {
            return Count(ContactQueries.ContactTable);
        }
    }
}