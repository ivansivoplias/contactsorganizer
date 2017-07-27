using Organizer.Common.Entities;
using Organizer.DAL.Helpers;
using Organizer.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Organizer.DAL.Repository
{
    public class PersonalInfoRepository : RepositoryBase<PersonalInfo>
    {
        public PersonalInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override void InsertCommandParameters(PersonalInfo entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{PersonalInfoQueries.PersonalInfoId}", entity.Id);
            cmd.Parameters.AddWithValue("@FirstName", GetValueOrDbNull(entity.FirstName));
            cmd.Parameters.AddWithValue("@Lastname", GetValueOrDbNull(entity.Lastname));
            cmd.Parameters.AddWithValue("@MiddleName", GetValueOrDbNull(entity.MiddleName));
            cmd.Parameters.AddWithValue("@Nickname", GetValueOrDbNull(entity.Nickname));
            cmd.Parameters.AddWithValue("@Email", GetValueOrDbNull(entity.Email));
        }

        protected override void UpdateCommandParameters(PersonalInfo entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{PersonalInfoQueries.PersonalInfoId}", entity.Id);
            cmd.Parameters.AddWithValue("@FirstName", GetValueOrDbNull(entity.FirstName));
            cmd.Parameters.AddWithValue("@Lastname", GetValueOrDbNull(entity.Lastname));
            cmd.Parameters.AddWithValue("@MiddleName", GetValueOrDbNull(entity.MiddleName));
            cmd.Parameters.AddWithValue("@Nickname", GetValueOrDbNull(entity.Nickname));
            cmd.Parameters.AddWithValue("@Email", GetValueOrDbNull(entity.Email));
        }

        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{PersonalInfoQueries.PersonalInfoId}", id);
        }

        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{PersonalInfoQueries.PersonalInfoId}", id);
        }

        protected override PersonalInfo Map(SqlDataReader reader)
        {
            PersonalInfo personalInfo = null;
            if (reader.HasRows)
            {
                personalInfo = new PersonalInfo();
                while (reader.Read())
                {
                    personalInfo.Id = Convert.ToInt32(reader[PersonalInfoQueries.PersonalInfoId].ToString());
                    personalInfo.FirstName = reader["FirstName"].ToString();
                    personalInfo.Lastname = reader["Lastname"].ToString();
                    personalInfo.MiddleName = reader["MiddleName"].ToString();
                    personalInfo.Nickname = reader["Nickname"].ToString();

                    personalInfo.Email = reader["Email"].ToString();
                }
            }
            return personalInfo;
        }

        protected override List<PersonalInfo> MapCollection(SqlDataReader reader)
        {
            List<PersonalInfo> personalInfoList = new List<PersonalInfo>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var personalInfo = new PersonalInfo();
                    personalInfo.Id = Convert.ToInt32(reader[PersonalInfoQueries.PersonalInfoId].ToString());
                    personalInfo.FirstName = reader["FirstName"].ToString();
                    personalInfo.Lastname = reader["Lastname"].ToString();
                    personalInfo.MiddleName = reader["MiddleName"].ToString();
                    personalInfo.Nickname = reader["Nickname"].ToString();
                    personalInfo.Email = reader["Email"].ToString();

                    personalInfoList.Add(personalInfo);
                }
            }
            return personalInfoList;
        }

        public override int Insert(PersonalInfo entity)
        {
            var query = PersonalInfoQueries.GetInsertQuery();
            return Insert(entity, query);
        }

        public override int Update(PersonalInfo entity)
        {
            var query = PersonalInfoQueries.GetUpdateQuery();
            return Update(entity, query);
        }

        public override int Delete(int id)
        {
            var query = PersonalInfoQueries.GetDeleteQuery();
            return Delete(id, query);
        }

        public override PersonalInfo GetById(int id)
        {
            var query = PersonalInfoQueries.GetGetByIdQuery();

            return GetById(id, query);
        }

        public override IEnumerable<PersonalInfo> GetAll()
        {
            return GetAll(PersonalInfoQueries.GetAllQuery());
        }

        public override int Count()
        {
            return Count(PersonalInfoQueries.PersonalInfoTable);
        }
    }
}