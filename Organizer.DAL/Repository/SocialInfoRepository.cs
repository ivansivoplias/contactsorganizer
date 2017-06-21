using Organizer.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organizer.Infrastructure.Database;
using System.Data.SqlClient;

namespace Organizer.DAL.Repository
{
    public class SocialInfoRepository : RepositoryBase<SocialInfo>
    {
        public SocialInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Passes the parameters for Insert Statement
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cmd"></param>
        protected override void InsertCommandParameters(SocialInfo entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@PrimaryPhone", entity.AppName);
            cmd.Parameters.AddWithValue("@AppName", entity.AppName);
            cmd.Parameters.AddWithValue("@AppId", entity.AppId);
        }

        /// <summary>
        /// Passes the parameters for Update Statement
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cmd"></param>
        protected override void UpdateCommandParameters(SocialInfo entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{entity.IdColumnName}", entity.Id);
            cmd.Parameters.AddWithValue("@AppName", entity.AppName);
            cmd.Parameters.AddWithValue("@AppId", entity.AppId);
        }

        /// <summary>
        /// Passes the parameters to command for Delete Statement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cmd"></param>
        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new SocialInfo().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        /// <summary>
        /// Passes the parameters to command for populate by key statement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cmd"></param>
        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new SocialInfo().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        /// <summary>
        /// Maps data for populate by key statement
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override SocialInfo Map(SqlDataReader reader)
        {
            var socialInfo = new SocialInfo();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    socialInfo.Id = Convert.ToInt32(reader[socialInfo.IdColumnName].ToString());
                    socialInfo.ContactId = Convert.ToInt32(reader["ContactId"].ToString());

                    socialInfo.AppName = reader["AppName"].ToString();
                    socialInfo.AppId = reader["AppId"].ToString();
                }
            }
            return socialInfo;
        }

        /// <summary>
        /// Maps data for populate all statement
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override List<SocialInfo> MapCollection(SqlDataReader reader)
        {
            var socials = new List<SocialInfo>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var socialInfo = new SocialInfo();
                    socialInfo.Id = Convert.ToInt32(reader[socialInfo.IdColumnName].ToString());
                    socialInfo.ContactId = Convert.ToInt32(reader["ContactId"].ToString());

                    socialInfo.AppName = reader["AppName"].ToString();
                    socialInfo.AppId = reader["AppId"].ToString();
                    socials.Add(socialInfo);
                }
            }
            return socials;
        }
    }
}