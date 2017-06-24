﻿using System;
using System.Collections.Generic;
using Organizer.Infrastructure.Database;
using System.Data.SqlClient;
using Organizer.Common.Entities;
using Organizer.DAL.Helpers;

namespace Organizer.DAL.Repository
{
    public class MeetingRepository : RepositoryBase<Meeting>, IMeetingRepository
    {
        public MeetingRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override void InsertCommandParameters(Meeting entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@Description", entity.Description);
            cmd.Parameters.AddWithValue("@MeetingName", entity.MeetingName);
            cmd.Parameters.AddWithValue("@MeetingDate", entity.MeetingDate);
            cmd.Parameters.AddWithValue("@NotificationDate", entity.NotificationDate);
            cmd.Parameters.AddWithValue("@SendNotifications", entity.SendNotifications);
            cmd.Parameters.AddWithValue("@UserId", entity.UserId);
        }

        protected override void UpdateCommandParameters(Meeting entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{MeetingQueries.MeetingId}", entity.Id);
            cmd.Parameters.AddWithValue("@MeetingName", entity.MeetingName);
            cmd.Parameters.AddWithValue("@Description", entity.Description);
            cmd.Parameters.AddWithValue("@MeetingDate", entity.MeetingDate);
            cmd.Parameters.AddWithValue("@NotificationDate", entity.NotificationDate);
            cmd.Parameters.AddWithValue("@SendNotifications", entity.SendNotifications);
            cmd.Parameters.AddWithValue("@UserId", entity.UserId);
        }

        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{MeetingQueries.MeetingId}", id);
        }

        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{MeetingQueries.MeetingId}", id);
        }

        protected override Meeting Map(SqlDataReader reader)
        {
            Meeting meeting = null;
            if (reader.HasRows)
            {
                meeting = new Meeting();
                while (reader.Read())
                {
                    meeting.Id = Convert.ToInt32(reader[MeetingQueries.MeetingId].ToString());
                    meeting.MeetingName = reader["MeetingName"].ToString();
                    meeting.Description = reader["Description"].ToString();
                    meeting.MeetingDate = DateTime.Parse(reader["Password"].ToString());
                    meeting.NotificationDate = DateTime.Parse(reader["NotificationDate"].ToString());
                    meeting.SendNotifications = bool.Parse(reader["SendNotifications"].ToString());
                    meeting.UserId = Convert.ToInt32(reader["UserId"].ToString());
                }
            }
            return meeting;
        }

        protected override List<Meeting> MapCollection(SqlDataReader reader)
        {
            List<Meeting> meetings = null;
            if (reader.HasRows)
            {
                meetings = new List<Meeting>();
                while (reader.Read())
                {
                    var meeting = new Meeting();
                    meeting.Id = Convert.ToInt32(reader[MeetingQueries.MeetingId].ToString());
                    meeting.MeetingName = reader["MeetingName"].ToString();
                    meeting.Description = reader["Description"].ToString();
                    meeting.MeetingDate = DateTime.Parse(reader["Password"].ToString());
                    meeting.NotificationDate = DateTime.Parse(reader["NotificationDate"].ToString());
                    meeting.SendNotifications = bool.Parse(reader["SendNotifications"].ToString());
                    meeting.UserId = Convert.ToInt32(reader["UserId"].ToString());
                    meetings.Add(meeting);
                }
            }
            return meetings;
        }

        public IEnumerable<Meeting> FilterByMeetingDate(int userId, DateTime meetingDate)
        {
            IEnumerable<Meeting> result = null;

            var query = MeetingQueries.GetFilterByMeetingDateQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId),
                    new SqlParameter("@MeetingDate", meetingDate));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Meeting> FilterByMeetingNameLike(int userId, string meetingName)
        {
            IEnumerable<Meeting> result = null;
            var query = MeetingQueries.GetFilterByMeetingName();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId),
                    new SqlParameter("@MeetingName", meetingName.MakeLikeExpression()));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public override int Insert(Meeting entity, SqlTransaction sqlTransaction)
        {
            var query = MeetingQueries.GetInsertQuery();
            return Insert(entity, query, sqlTransaction);
        }

        public override int Update(Meeting entity, SqlTransaction sqlTransaction)
        {
            var query = MeetingQueries.GetUpdateQuery();
            return Update(entity, query, sqlTransaction);
        }

        public override int Delete(int id, SqlTransaction sqlTransaction)
        {
            var query = MeetingQueries.GetDeleteQuery();
            return Delete(id, query, sqlTransaction);
        }

        public override Meeting GetById(int id)
        {
            var query = MeetingQueries.GetGetByIdQuery();

            return GetById(id, query);
        }

        public override IEnumerable<Meeting> GetAll()
        {
            return GetAll(MeetingQueries.GetAllQuery());
        }

        public IEnumerable<Meeting> GetUserMeetings(int userId)
        {
            IEnumerable<Meeting> result = null;
            var query = MeetingQueries.GetUserMeetingsQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }
    }
}