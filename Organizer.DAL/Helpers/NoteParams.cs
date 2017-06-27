using Organizer.Common.Enums;
using System;
using System.Data.SqlClient;

namespace Organizer.DAL.Helpers
{
    public static class NoteParams
    {
        public static SqlParameter[] GetFilterByCreationDateParams(int userId, DateTime date)
        {
            return new SqlParameter[]
            {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@CreationDate", date)
            };
        }

        public static SqlParameter[] GetFilterByLastChangeDateParams(int userId, DateTime lastChangeDate)
        {
            return new SqlParameter[]
            {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@LastChangeDate", lastChangeDate)
            };
        }

        public static SqlParameter[] GetFilterByNoteTypeParams(int userId, NoteType noteType)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@NoteType", noteType.ToString())
            };
        }

        public static SqlParameter[] GetFilterByStateParams(int userId, State state)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@State", state.ToString())
            };
        }

        public static SqlParameter[] GetFilterByPriorityParams(int userId, Priority priority)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Priority", priority.ToString())
            };
        }

        public static SqlParameter[] GetFilterByCreationBetweenParams(int userId, DateTime startLimit, DateTime endLimit)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@StartLimit", startLimit),
                new SqlParameter("@EndLimit", endLimit)
            };
        }

        public static SqlParameter[] GetFilterByStartDateParams(int userId, DateTime startDate)
        {
            return new SqlParameter[]
            {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@StartDate", startDate)
            };
        }

        public static SqlParameter[] GetFilterByEndDateParams(int userId, DateTime endDate)
        {
            return new SqlParameter[]
            {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@EndDate", endDate)
            };
        }

        public static SqlParameter[] GetFilterByCaptionParams(int userId, string caption)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Caption", caption.MakeLikeExpression())
            };
        }

        public static SqlParameter[] GetGetUserNotesParams(int userId)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId)
            };
        }
    }
}