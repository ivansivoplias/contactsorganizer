using System;
using System.Data.SqlClient;

namespace Organizer.DAL.Helpers
{
    public static class MeetingParams
    {
        public static SqlParameter[] GetFilterByMeetingDateParams(int userId, DateTime meetingDate)
        {
            return new SqlParameter[]
            {
                 new SqlParameter("@UserId", userId),
                 new SqlParameter("@MeetingDate", meetingDate)
            };
        }

        public static SqlParameter[] GetFilterByMeetingNameParams(int userId, string meetingName)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@MeetingName", meetingName.MakeLikeExpression())
            };
        }

        public static SqlParameter[] GetFindByMeetingNameParams(int userId, string meetingName)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@MeetingName", meetingName)
            };
        }

        public static SqlParameter[] GetGetUserMeetingsParams(int userId)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId)
            };
        }
    }
}