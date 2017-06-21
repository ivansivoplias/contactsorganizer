using System;
using System.Collections.Generic;
using Organizer.Infrastructure.Database;
using System.Data.SqlClient;
using Organizer.Common.Entities;

namespace Organizer.DAL.Repository
{
    public class PersonalInfoRepository : RepositoryBase<PersonalInfo>
    {
        private readonly string _personalInfoTable;
        private readonly string _personalInfoId;

        public PersonalInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            var pers = new PersonalInfo();
            _personalInfoId = pers.IdColumnName;
            _personalInfoTable = pers.TableName;
        }

        protected override void InsertCommandParameters(PersonalInfo entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{entity.IdColumnName}", entity.Id);
            cmd.Parameters.AddWithValue("@FirstName", entity.FirstName);
            cmd.Parameters.AddWithValue("@Lastname", entity.Lastname);
            cmd.Parameters.AddWithValue("@MiddleName", entity.MiddleName);
            cmd.Parameters.AddWithValue("@Nickname", entity.Nickname);
            cmd.Parameters.AddWithValue("@Email", entity.Email);
        }

        protected override void UpdateCommandParameters(PersonalInfo entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{entity.IdColumnName}", entity.Id);
            cmd.Parameters.AddWithValue("@FirstName", entity.FirstName);
            cmd.Parameters.AddWithValue("@Lastname", entity.Lastname);
            cmd.Parameters.AddWithValue("@MiddleName", entity.MiddleName);
            cmd.Parameters.AddWithValue("@Nickname", entity.Nickname);
            cmd.Parameters.AddWithValue("@Email", entity.Email);
        }

        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new PersonalInfo().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new PersonalInfo().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        protected override PersonalInfo Map(SqlDataReader reader)
        {
            var personalInfo = new PersonalInfo();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    personalInfo.Id = Convert.ToInt32(reader[personalInfo.IdColumnName].ToString());
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
            var personalInfoList = new List<PersonalInfo>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var personalInfo = new PersonalInfo();
                    personalInfo.Id = Convert.ToInt32(reader[personalInfo.IdColumnName].ToString());
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

        public override int Insert(PersonalInfo entity, SqlTransaction sqlTransaction)
        {
            var query = $"INSERT INTO {_personalInfoTable} ({_personalInfoId}, FirstName, Lastname, MiddleName, " +
                "Nickname, Email)" +
                $" VALUES(@{_personalInfoId}, @FirstName, @Lastname, @MiddleName, " +
                "@Nickname, @Email)";
            return Insert(entity, query, sqlTransaction);
        }

        public override int Update(PersonalInfo entity, SqlTransaction sqlTransaction)
        {
            var query = $"UPDATE {_personalInfoTable} SET FirstName = @FirstName," +
                " Lastname = @Lastname, MiddleName = @MiddleName, " +
                "Nickname = @Nickname, Email = @Email" +
                $" WHERE {_personalInfoId} = @{_personalInfoId}";
            return Update(entity, query, sqlTransaction);
        }

        public override int Delete(int id, SqlTransaction sqlTransaction)
        {
            var query = $"DELETE FROM {_personalInfoTable} WHERE {_personalInfoId} = @{_personalInfoId}";
            return Delete(id, query, sqlTransaction);
        }

        public override PersonalInfo GetById(int id)
        {
            var query = $"SELECT TOP 1 * FROM {_personalInfoTable} WHERE {_personalInfoId} = @{_personalInfoId}";

            return GetById(id, query);
        }

        public override IEnumerable<PersonalInfo> GetAll()
        {
            return GetAll($"SELECT * FROM {_personalInfoTable}");
        }
    }
}