using System;
using System.Collections.Generic;
using Organizer.Infrastructure.Database;
using System.Data.SqlClient;
using Organizer.Common.Entities;
using Organizer.DAL.Helpers;

namespace Organizer.DAL.Repository
{
    public class MeetingRepository : RepositoryBase<Meeting>, IMeetingRepository
    {
        private readonly string _meetingId;
        private readonly string _meetingTable;

        public MeetingRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            var meeting = new Meeting();
            _meetingId = meeting.IdColumnName;
            _meetingTable = meeting.TableName;
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
            cmd.Parameters.AddWithValue($"@{entity.IdColumnName}", entity.Id);
            cmd.Parameters.AddWithValue("@MeetingName", entity.MeetingName);
            cmd.Parameters.AddWithValue("@Description", entity.Description);
            cmd.Parameters.AddWithValue("@MeetingDate", entity.MeetingDate);
            cmd.Parameters.AddWithValue("@NotificationDate", entity.NotificationDate);
            cmd.Parameters.AddWithValue("@SendNotifications", entity.SendNotifications);
            cmd.Parameters.AddWithValue("@UserId", entity.UserId);
        }

        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new Meeting().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new Meeting().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        protected override Meeting Map(SqlDataReader reader)
        {
            Meeting meeting = null;
            if (reader.HasRows)
            {
                meeting = new Meeting();
                while (reader.Read())
                {
                    meeting.Id = Convert.ToInt32(reader[meeting.IdColumnName].ToString());
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
                    meeting.Id = Convert.ToInt32(reader[meeting.IdColumnName].ToString());
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

            var meetingTable = new Meeting().TableName;
            var query = $"SELECT * FROM {meetingTable} "
                + "WHERE UserId = @UserId AND MeetingDate = @MeetingDate";

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId), new SqlParameter("@MeetingDate", meetingDate));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Meeting> FilterByMeetingName(int userId, string meetingName)
        {
            IEnumerable<Meeting> result = null;

            var meetingTable = new Meeting().TableName;
            var query = $"SELECT * FROM {meetingTable} "
                + "WHERE UserId = @UserId AND MeetingName LIKE @MeetingName";

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
            var query = $"INSERT INTO {_meetingTable} (Description, MeetingName, MeetingDate, " +
                "NotificationDate, SendNotifications, UserId)" +
                " VALUES(@Description, @MeetingName, @MeetingDate, " +
                "@NotificationDate, @SendNotifications, @UserId)";
            return Insert(entity, query, sqlTransaction);
        }

        public override int Update(Meeting entity, SqlTransaction sqlTransaction)
        {
            var query = $"UPDATE {_meetingTable} SET MeetingName = @MeetingName," +
                " Description = @Description, MeetingDate = @MeetingDate, " +
                "NotificationDate = @NotificationDate, SendNotifications = @SendNotifications," +
                " UserId = @Userid" +
                $" WHERE {_meetingId} = @{_meetingId}";
            return Update(entity, query, sqlTransaction);
        }

        public override int Delete(int id, SqlTransaction sqlTransaction)
        {
            var query = $"DELETE FROM {_meetingTable} WHERE {_meetingId} = @{_meetingId}";
            return Delete(id, query, sqlTransaction);
        }

        public override Meeting GetById(int id)
        {
            var query = $"SELECT TOP 1 * FROM {_meetingTable} WHERE {_meetingId} = @{_meetingId}";

            return GetById(id, query);
        }

        public override IEnumerable<Meeting> GetAll()
        {
            return GetAll($"SELECT * FROM {_meetingTable}");
        }
    }
}