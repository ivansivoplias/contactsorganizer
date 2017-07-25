using Organizer.Common.Entities;
using Organizer.Common.Enums;
using Organizer.DAL.Helpers;
using Organizer.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
            cmd.Parameters.AddWithValue("@MeetingType", entity.MeetingType.ToString());
            cmd.Parameters.AddWithValue("@MeetingDate", entity.MeetingDate);
            cmd.Parameters.AddWithValue("@NotificationDate", entity.NotificationDate);
            cmd.Parameters.AddWithValue("@SendNotifications", entity.SendNotifications);
            cmd.Parameters.AddWithValue("@NotificationTime", entity.NotificationTime);
            cmd.Parameters.AddWithValue("@MeetingPlace", entity.MeetingPlace);
            cmd.Parameters.AddWithValue("@MeetingTime", entity.MeetingTime);
            cmd.Parameters.AddWithValue("@UserId", entity.UserId);
        }

        protected override void UpdateCommandParameters(Meeting entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{MeetingQueries.MeetingId}", entity.Id);
            cmd.Parameters.AddWithValue("@MeetingName", entity.MeetingName);
            cmd.Parameters.AddWithValue("@Description", entity.Description);
            cmd.Parameters.AddWithValue("@MeetingType", entity.MeetingType.ToString());
            cmd.Parameters.AddWithValue("@MeetingDate", entity.MeetingDate);
            cmd.Parameters.AddWithValue("@NotificationDate", entity.NotificationDate);
            cmd.Parameters.AddWithValue("@SendNotifications", entity.SendNotifications);
            cmd.Parameters.AddWithValue("@NotificationTime", entity.NotificationTime);
            cmd.Parameters.AddWithValue("@MeetingPlace", entity.MeetingPlace);
            cmd.Parameters.AddWithValue("@MeetingTime", entity.MeetingTime);
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

                    TryParseEnum<MeetingType>(reader, "MeetingType", (x) => meeting.MeetingType = x);

                    meeting.MeetingDate = DateTime.Parse(reader["MeetingDate"].ToString());
                    meeting.NotificationDate = DateTime.Parse(reader["NotificationDate"].ToString());
                    meeting.SendNotifications = bool.Parse(reader["SendNotifications"].ToString());
                    meeting.NotificationTime = TimeSpan.Parse(reader["NotificationTime"].ToString());
                    meeting.MeetingPlace = reader["MeetingPlace"].ToString();
                    meeting.MeetingTime = TimeSpan.Parse(reader["MeetingTime"].ToString());
                    meeting.UserId = Convert.ToInt32(reader["UserId"].ToString());
                }
            }
            return meeting;
        }

        protected override List<Meeting> MapCollection(SqlDataReader reader)
        {
            var meetings = new List<Meeting>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var meeting = new Meeting();
                    meeting.Id = Convert.ToInt32(reader[MeetingQueries.MeetingId].ToString());
                    meeting.MeetingName = reader["MeetingName"].ToString();
                    meeting.Description = reader["Description"].ToString();

                    TryParseEnum<MeetingType>(reader, "MeetingType", (x) => meeting.MeetingType = x);

                    meeting.MeetingDate = DateTime.Parse(reader["MeetingDate"].ToString());
                    meeting.NotificationDate = DateTime.Parse(reader["NotificationDate"].ToString());
                    meeting.SendNotifications = bool.Parse(reader["SendNotifications"].ToString());
                    meeting.NotificationTime = TimeSpan.Parse(reader["NotificationTime"].ToString());
                    meeting.MeetingPlace = reader["MeetingPlace"].ToString();
                    meeting.MeetingTime = TimeSpan.Parse(reader["MeetingTime"].ToString());
                    meeting.UserId = Convert.ToInt32(reader["UserId"].ToString());
                    meetings.Add(meeting);
                }
            }
            return meetings;
        }

        public IEnumerable<Meeting> FilterByMeetingDate(int userId, DateTime meetingDate, int? pageSize = null, int? page = null)
        {
            IEnumerable<Meeting> result = null;

            var query = MeetingQueries.GetFilterByMeetingDateQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("MeetingDate", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, MeetingParams.GetFilterByMeetingDateParams(userId, meetingDate));

                if (_unitOfWork.Transaction != null)
                {
                    cmd.Transaction = _unitOfWork.Transaction;
                }

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Meeting> FilterByMeetingNameLike(int userId, string meetingName, int? pageSize = null, int? page = null)
        {
            IEnumerable<Meeting> result = null;
            var query = MeetingQueries.GetFilterByMeetingName();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("MeetingName", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, MeetingParams.GetFilterByMeetingNameParams(userId, meetingName));

                if (_unitOfWork.Transaction != null)
                {
                    cmd.Transaction = _unitOfWork.Transaction;
                }

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

        public IEnumerable<Meeting> GetUserMeetings(int userId, int? pageSize = null, int? page = null)
        {
            IEnumerable<Meeting> result = null;
            var query = MeetingQueries.GetUserMeetingsQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging(MeetingQueries.MeetingId, pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, MeetingParams.GetGetUserMeetingsParams(userId));

                if (_unitOfWork.Transaction != null)
                {
                    cmd.Transaction = _unitOfWork.Transaction;
                }

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public Meeting FindByMeetingName(int userId, string meetingName)
        {
            Meeting result = null;
            var query = MeetingQueries.GetFindByMeetingNameQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, MeetingParams.GetFindByMeetingNameParams(userId, meetingName));

                if (_unitOfWork.Transaction != null)
                {
                    cmd.Transaction = _unitOfWork.Transaction;
                }

                using (var reader = cmd.ExecuteReader())
                {
                    result = Map(reader);
                }
            }

            return result;
        }

        public override int Count()
        {
            return Count(MeetingQueries.MeetingTable);
        }
    }
}