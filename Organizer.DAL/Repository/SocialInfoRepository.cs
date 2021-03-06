﻿using Organizer.Common.Entities;
using Organizer.DAL.Helpers;
using Organizer.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Organizer.DAL.Repository
{
    public class SocialInfoRepository : RepositoryBase<SocialInfo>, ISocialInfoRepository
    {
        public SocialInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override void InsertCommandParameters(SocialInfo entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@ContactId", entity.ContactId);
            cmd.Parameters.AddWithValue("@AppName", entity.AppName);
            cmd.Parameters.AddWithValue("@AppId", entity.AppId);
        }

        protected override void UpdateCommandParameters(SocialInfo entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{SocialInfoQueries.SocialInfoId}", entity.Id);
            cmd.Parameters.AddWithValue("@AppName", entity.AppName);
            cmd.Parameters.AddWithValue("@AppId", entity.AppId);
        }

        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{SocialInfoQueries.SocialInfoId}", id);
        }

        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{SocialInfoQueries.SocialInfoId}", id);
        }

        protected override SocialInfo Map(SqlDataReader reader)
        {
            SocialInfo socialInfo = null;
            if (reader.HasRows)
            {
                socialInfo = new SocialInfo();
                while (reader.Read())
                {
                    socialInfo.Id = Convert.ToInt32(reader[SocialInfoQueries.SocialInfoId].ToString());
                    socialInfo.ContactId = Convert.ToInt32(reader["ContactId"].ToString());

                    socialInfo.AppName = reader["AppName"].ToString();
                    socialInfo.AppId = reader["AppId"].ToString();
                }
            }
            return socialInfo;
        }

        protected override List<SocialInfo> MapCollection(SqlDataReader reader)
        {
            List<SocialInfo> socials = new List<SocialInfo>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var socialInfo = new SocialInfo();
                    socialInfo.Id = Convert.ToInt32(reader[SocialInfoQueries.SocialInfoId].ToString());
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
            var query = SocialInfoQueries.GetInsertQuery();
            return Insert(entity, query, sqlTransaction);
        }

        public override int Update(SocialInfo entity, SqlTransaction sqlTransaction)
        {
            var query = SocialInfoQueries.GetUpdateQuery();
            return Update(entity, query, sqlTransaction);
        }

        public override int Delete(int id, SqlTransaction sqlTransaction)
        {
            var query = SocialInfoQueries.GetDeleteQuery();
            return Delete(id, query, sqlTransaction);
        }

        public override SocialInfo GetById(int id)
        {
            var query = SocialInfoQueries.GetGetByIdQuery();

            return GetById(id, query);
        }

        public override IEnumerable<SocialInfo> GetAll()
        {
            return GetAll(SocialInfoQueries.GetAllQuery());
        }

        public IEnumerable<SocialInfo> GetContactSocials(int contactId, int? pageSize = null, int? page = null)
        {
            IEnumerable<SocialInfo> list = null;
            var query = SocialInfoQueries.GetContactSocialsQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging(SocialInfoQueries.SocialInfoId, pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, SocialInfoParams.GetGetContactSocialsParams(contactId));

                if (_unitOfWork.Transaction != null)
                {
                    cmd.Transaction = _unitOfWork.Transaction;
                }

                using (var reader = cmd.ExecuteReader())
                {
                    list = MapCollection(reader);
                }
            }

            return list;
        }

        public override int Count()
        {
            return Count(SocialInfoQueries.SocialInfoTable);
        }

        public IEnumerable<SocialInfo> GetSocialsByAppName(int contactId, string appName, int? pageSize = default(int?), int? page = default(int?))
        {
            IEnumerable<SocialInfo> list = null;
            var query = SocialInfoQueries.GetSocialsByAppNameQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging(SocialInfoQueries.SocialInfoId, pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, SocialInfoParams.GetSocialsByAppNameParams(contactId, appName));

                if (_unitOfWork.Transaction != null)
                {
                    cmd.Transaction = _unitOfWork.Transaction;
                }

                using (var reader = cmd.ExecuteReader())
                {
                    list = MapCollection(reader);
                }
            }

            return list;
        }

        public SocialInfo FindSocial(int contactId, string appName, string appId)
        {
            SocialInfo social = null;
            var query = SocialInfoQueries.GetFindSocialQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, SocialInfoParams.GetFindSocialParams(contactId, appName, appId));

                if (_unitOfWork.Transaction != null)
                {
                    cmd.Transaction = _unitOfWork.Transaction;
                }

                using (var reader = cmd.ExecuteReader())
                {
                    social = Map(reader);
                }
            }

            return social;
        }
    }
}