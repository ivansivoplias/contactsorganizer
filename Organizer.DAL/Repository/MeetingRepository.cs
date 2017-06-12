﻿using System;
using System.Collections.Generic;
using Organizer.Infrastructure.Database;
using System.Data.SqlClient;
using Organizer.Common.Entities;

namespace Organizer.DAL.Repository
{
    public class MeetingRepository : RepositoryBase<Meeting>
    {
        public MeetingRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Passes the parameters for Insert Statement
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cmd"></param>
        protected override void InsertCommandParameters(Meeting entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@Description", entity.Description);
            cmd.Parameters.AddWithValue("@MeetingDate", entity.MeetingDate);
            cmd.Parameters.AddWithValue("@NotificationDate", entity.NotificationDate);
            cmd.Parameters.AddWithValue("@SendNotifications", entity.SendNotifications);
            cmd.Parameters.AddWithValue("@UserId", entity.UserId);
        }

        /// <summary>
        /// Passes the parameters for Update Statement
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cmd"></param>
        protected override void UpdateCommandParameters(Meeting entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{entity.IdColumnName}", entity.Id);
            cmd.Parameters.AddWithValue("@Description", entity.Description);
            cmd.Parameters.AddWithValue("@MeetingDate", entity.MeetingDate);
            cmd.Parameters.AddWithValue("@NotificationDate", entity.NotificationDate);
            cmd.Parameters.AddWithValue("@SendNotifications", entity.SendNotifications);
            cmd.Parameters.AddWithValue("@UserId", entity.UserId);
        }

        /// <summary>
        /// Passes the parameters to command for Delete Statement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cmd"></param>
        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new Meeting().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        /// <summary>
        /// Passes the parameters to command for populate by key statement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cmd"></param>
        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new Meeting().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        /// <summary>
        /// Maps data for populate by key statement
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override Meeting Map(SqlDataReader reader)
        {
            var meeting = new Meeting();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    meeting.Id = Convert.ToInt32(reader[meeting.IdColumnName].ToString());
                    meeting.Description = reader["Description"].ToString();
                    meeting.MeetingDate = DateTime.Parse(reader["Password"].ToString());
                    meeting.NotificationDate = DateTime.Parse(reader["NotificationDate"].ToString());
                    meeting.SendNotifications = bool.Parse(reader["SendNotifications"].ToString());
                    meeting.UserId = Convert.ToInt32(reader["UserId"].ToString());
                }
            }
            return meeting;
        }

        /// <summary>
        /// Maps data for populate all statement
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override List<Meeting> MapCollection(SqlDataReader reader)
        {
            var meetings = new List<Meeting>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var meeting = new Meeting();
                    meeting.Id = Convert.ToInt32(reader[meeting.IdColumnName].ToString());
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
    }
}