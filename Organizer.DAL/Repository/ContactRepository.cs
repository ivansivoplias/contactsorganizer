using System;
using System.Collections.Generic;
using Organizer.Infrastructure.Database;
using System.Data.SqlClient;
using Organizer.Common.Entities;

namespace Organizer.DAL.Repository
{
    public class ContactRepository : RepositoryBase<Contact>
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
            string secondaryPhones = string.Join(",", entity.SecondaryPhones);
            cmd.Parameters.AddWithValue("@SecondaryPhones", entity.SecondaryPhones);
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
            string secondaryPhones = string.Join(",", entity.SecondaryPhones);
            cmd.Parameters.AddWithValue("@SecondaryPhones", entity.SecondaryPhones);
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
                    contact.SecondaryPhones = reader["SecondaryPhones"].ToString().Split(',');
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
                    contact.SecondaryPhones = reader["SecondaryPhones"].ToString().Split(',');
                    contact.UserId = Convert.ToInt32(reader["UserId"].ToString());
                    contacts.Add(contact);
                }
            }
            return contacts;
        }
    }
}