using Organizer.Common.Entities;
using System;
using System.Collections.Generic;
using Organizer.Infrastructure.Database;
using System.Data.SqlClient;

namespace Organizer.DAL.Repository
{
    public class SocialInfoRepository : RepositoryBase<SocialInfo>
    {
        private readonly string _socialInfoId;
        private readonly string _socialInfoTable;

        public SocialInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            var social = new SocialInfo();
            _socialInfoId = social.IdColumnName;
            _socialInfoTable = social.TableName;
        }

        protected override void InsertCommandParameters(SocialInfo entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@ContactId", entity.ContactId);
            cmd.Parameters.AddWithValue("@AppName", entity.AppName);
            cmd.Parameters.AddWithValue("@AppId", entity.AppId);
        }

        protected override void UpdateCommandParameters(SocialInfo entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{entity.IdColumnName}", entity.Id);
            cmd.Parameters.AddWithValue("@AppName", entity.AppName);
            cmd.Parameters.AddWithValue("@AppId", entity.AppId);
        }

        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new SocialInfo().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new SocialInfo().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

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

        public override int Insert(SocialInfo entity, SqlTransaction sqlTransaction)
        {
            var query = $"INSERT INTO {_socialInfoTable} (ContactId, AppName, AppId)" +
                " VALUES(@ContactId, @AppName, @AppId)";
            return Insert(entity, query, sqlTransaction);
        }

        public override int Update(SocialInfo entity, SqlTransaction sqlTransaction)
        {
            var query = $"UPDATE {_socialInfoTable} SET AppName = @AppName, AppId = @AppId" +
                $" WHERE {_socialInfoId} = @{_socialInfoId}";
            return Update(entity, query, sqlTransaction);
        }

        public override int Delete(int id, SqlTransaction sqlTransaction)
        {
            var query = $"DELETE FROM {_socialInfoTable} WHERE {_socialInfoId} = @{_socialInfoId}";
            return Delete(id, query, sqlTransaction);
        }

        public override SocialInfo GetById(int id)
        {
            var query = $"SELECT TOP 1 * FROM {_socialInfoTable} WHERE {_socialInfoId} = @{_socialInfoId}";

            return GetById(id, query);
        }

        public override IEnumerable<SocialInfo> GetAll()
        {
            return GetAll($"SELECT * FROM {_socialInfoTable}");
        }
    }
}