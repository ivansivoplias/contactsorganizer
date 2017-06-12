using System;
using System.Collections.Generic;
using Organizer.Infrastructure.Database;
using System.Data.SqlClient;
using Organizer.Common.Entities;
using System.Text;
using System.Linq;

namespace Organizer.DAL.Repository
{
    public class PersonalInfoRepository : RepositoryBase<PersonalInfo>
    {
        public PersonalInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Passes the parameters for Insert Statement
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cmd"></param>
        protected override void InsertCommandParameters(PersonalInfo entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{entity.IdColumnName}", entity.Id);
            cmd.Parameters.AddWithValue("@FirstName", entity.FirstName);
            cmd.Parameters.AddWithValue("@Lastname", entity.Lastname);
            cmd.Parameters.AddWithValue("@MiddleName", entity.MiddleName);
            cmd.Parameters.AddWithValue("@Nickname", entity.Nickname);

            cmd.Parameters.AddWithValue("@AdditionalContacts", GetCsvStringFromDictionary(entity.AdditionalContacts));

            cmd.Parameters.AddWithValue("@Email", entity.Email);
        }

        /// <summary>
        /// Passes the parameters for Update Statement
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cmd"></param>
        protected override void UpdateCommandParameters(PersonalInfo entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{entity.IdColumnName}", entity.Id);
            cmd.Parameters.AddWithValue("@FirstName", entity.FirstName);
            cmd.Parameters.AddWithValue("@Lastname", entity.Lastname);
            cmd.Parameters.AddWithValue("@MiddleName", entity.MiddleName);
            cmd.Parameters.AddWithValue("@Nickname", entity.Nickname);
            cmd.Parameters.AddWithValue("@AdditionalContacts", GetCsvStringFromDictionary(entity.AdditionalContacts));
            cmd.Parameters.AddWithValue("@Email", entity.Email);
        }

        /// <summary>
        /// Passes the parameters to command for Delete Statement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cmd"></param>
        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new PersonalInfo().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        /// <summary>
        /// Passes the parameters to command for populate by key statement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cmd"></param>
        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new PersonalInfo().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        /// <summary>
        /// Maps data for populate by key statement
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
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

                    var i = reader.GetOrdinal("AdditionalContacts");

                    if (!reader.IsDBNull(i))
                    {
                        personalInfo.AdditionalContacts = GetDictionaryFromCsvString(reader["AdditionalContacts"].ToString());
                    }

                    personalInfo.Email = reader["Email"].ToString();
                }
            }
            return personalInfo;
        }

        /// <summary>
        /// Maps data for populate all statement
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
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

                    var i = reader.GetOrdinal("AdditionalContacts");

                    if (!reader.IsDBNull(i))
                    {
                        personalInfo.AdditionalContacts = GetDictionaryFromCsvString(reader["AdditionalContacts"].ToString());
                    }

                    personalInfo.Email = reader["Email"].ToString();

                    personalInfoList.Add(personalInfo);
                }
            }
            return personalInfoList;
        }

        private string GetCsvStringFromDictionary(Dictionary<string, string> dictionary)
        {
            string result = string.Empty;
            if (dictionary != null && dictionary.Any())
            {
                var i = 0;
                var stringBuilder = new StringBuilder();
                foreach (var item in dictionary)
                {
                    stringBuilder.Append($"{item.Key}={item.Value}");
                    if (i != dictionary.Count - 1)
                    {
                        stringBuilder.Append(",");
                    }
                    i++;
                }

                result = stringBuilder.ToString();
            }
            return result;
        }

        private Dictionary<string, string> GetDictionaryFromCsvString(string csvString)
        {
            var result = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(csvString))
            {
                var keyValuePairs = csvString.Split(',');
                foreach (var pair in keyValuePairs)
                {
                    var keyValues = pair.Split('=');
                    result.Add(keyValues[0], keyValues[1]);
                }
            }

            return result;
        }
    }
}