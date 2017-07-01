using Organizer.Common.Enums;
using System;
using System.Data.SqlClient;

namespace Organizer.DAL.Helpers
{
    public static class NoteParams
    {
        public static SqlParameter[] GetFilterByCreationDateParams(int userId, DateTime date, NoteType noteType)
        {
            return new SqlParameter[]
            {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@CreationDate", date.Date),
                    new SqlParameter("@NoteType", noteType.ToString())
            };
        }

        public static SqlParameter[] GetFilterByLastChangeDateParams(int userId, DateTime lastChangeDate, NoteType noteType)
        {
            return new SqlParameter[]
            {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@LastChangeDate", lastChangeDate.Date),
                    new SqlParameter("@NoteType", noteType.ToString())
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

        public static SqlParameter[] GetFilterByStateParams(int userId, State state, NoteType noteType)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@State", state.ToString()),
                new SqlParameter("@NoteType", noteType.ToString())
            };
        }

        public static SqlParameter[] GetFilterByPriorityParams(int userId, Priority priority, NoteType noteType)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Priority", priority.ToString()),
                new SqlParameter("@NoteType", noteType.ToString())
            };
        }

        public static SqlParameter[] GetFilterByCreationBetweenParams(int userId, DateTime startLimit, DateTime endLimit, NoteType noteType)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@StartLimit", startLimit.Date),
                new SqlParameter("@EndLimit", endLimit.Date),
                new SqlParameter("@NoteType", noteType.ToString())
            };
        }

        public static SqlParameter[] GetFilterByStartDateParams(int userId, DateTime startDate, NoteType noteType)
        {
            return new SqlParameter[]
            {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@StartDate", startDate.Date),
                    new SqlParameter("@NoteType", noteType.ToString())
            };
        }

        public static SqlParameter[] GetFilterByEndDateParams(int userId, DateTime endDate, NoteType noteType)
        {
            return new SqlParameter[]
            {
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@EndDate", endDate.Date),
                    new SqlParameter("@NoteType", noteType.ToString())
            };
        }

        public static SqlParameter[] GetFilterByCaptionParams(int userId, string caption, NoteType noteType)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Caption", caption.MakeLikeExpression()),
                new SqlParameter("@NoteType", noteType.ToString())
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