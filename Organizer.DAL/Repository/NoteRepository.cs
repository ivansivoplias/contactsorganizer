using System;
using System.Collections.Generic;
using Organizer.Infrastructure.Database;
using System.Data.SqlClient;
using Organizer.Common.Entities;
using Organizer.Common.Enums;
using Organizer.DAL.Helpers;

namespace Organizer.DAL.Repository
{
    public class NoteRepository : RepositoryBase<Note>, INoteRepository
    {
        public NoteRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override void InsertCommandParameters(Note entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@Caption", entity.Caption);
            cmd.Parameters.AddWithValue("@NoteText", entity.NoteText);
            cmd.Parameters.AddWithValue("@CreationDate", entity.CreationDate);
            cmd.Parameters.AddWithValue("@LastChangeDate", entity.LastChangeDate);
            cmd.Parameters.AddWithValue("@NoteType", entity.NoteType.ToString());

            object state = GetValueOrDbNull(entity.State?.ToString());
            object priority = GetValueOrDbNull(entity.Priority?.ToString());
            object startDate = GetValueOrDbNull(entity.StartDate);
            object endDate = GetValueOrDbNull(entity.EndDate);

            cmd.Parameters.AddWithValue("@State", state);
            cmd.Parameters.AddWithValue("@Priority", priority);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            cmd.Parameters.AddWithValue("@UserId", entity.UserId);
        }

        protected override void UpdateCommandParameters(Note entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{NoteQueries.NoteId}", entity.Id);
            cmd.Parameters.AddWithValue("@Caption", entity.Caption);
            cmd.Parameters.AddWithValue("@NoteText", entity.NoteText);
            cmd.Parameters.AddWithValue("@CreationDate", entity.CreationDate);
            cmd.Parameters.AddWithValue("@LastChangeDate", entity.LastChangeDate);
            cmd.Parameters.AddWithValue("@NoteType", entity.NoteType.ToString());

            object state = GetValueOrDbNull(entity.State?.ToString());
            object priority = GetValueOrDbNull(entity.Priority?.ToString());
            object startDate = GetValueOrDbNull(entity.StartDate);
            object endDate = GetValueOrDbNull(entity.EndDate);

            cmd.Parameters.AddWithValue("@State", state);
            cmd.Parameters.AddWithValue("@Priority", priority);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
        }

        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{NoteQueries.NoteId}", id);
        }

        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{NoteQueries.NoteId}", id);
        }

        protected override Note Map(SqlDataReader reader)
        {
            Note note = null;
            if (reader.HasRows)
            {
                note = new Note();

                while (reader.Read())
                {
                    note.Id = Convert.ToInt32(reader[NoteQueries.NoteId].ToString());

                    TryParseDateTime(reader, "CreationDate", (x) => note.CreationDate = x);

                    TryParseDateTime(reader, "LastChangeDate", (x) => note.LastChangeDate = x);

                    note.NoteText = reader["NoteText"].ToString();

                    note.Caption = reader["Caption"].ToString();

                    TryParseEnum<NoteType>(reader, "NoteType", (x) => note.NoteType = x);

                    TryParseEnum<State>(reader, "State", (x) => note.State = x);

                    TryParseEnum<Priority>(reader, "Priority", (x) => note.Priority = x);

                    TryParseDateTime(reader, "StartDate", (x) => note.StartDate = x);

                    TryParseDateTime(reader, "EndDate", (x) => note.EndDate = x);

                    note.UserId = Convert.ToInt32(reader["UserId"].ToString());
                }
            }
            return note;
        }

        protected override List<Note> MapCollection(SqlDataReader reader)
        {
            List<Note> notes = null;
            if (reader.HasRows)
            {
                notes = new List<Note>();
                while (reader.Read())
                {
                    var note = new Note();
                    note.Id = Convert.ToInt32(reader[NoteQueries.NoteId].ToString());

                    TryParseDateTime(reader, "CreationDate", (x) => note.CreationDate = x);

                    TryParseDateTime(reader, "LastChangeDate", (x) => note.LastChangeDate = x);

                    note.NoteText = reader["NoteText"].ToString();

                    note.Caption = reader["Caption"].ToString();

                    TryParseEnum<NoteType>(reader, "NoteType", (x) => note.NoteType = x);

                    TryParseEnum<State>(reader, "State", (x) => note.State = x);

                    TryParseEnum<Priority>(reader, "Priority", (x) => note.Priority = x);

                    TryParseDateTime(reader, "StartDate", (x) => note.StartDate = x);

                    TryParseDateTime(reader, "EndDate", (x) => note.EndDate = x);

                    note.UserId = Convert.ToInt32(reader["UserId"].ToString());
                    notes.Add(note);
                }
            }
            return notes;
        }

        private object GetValueOrDbNull(object value)
        {
            return value ?? DBNull.Value;
        }

        private void TryParseEnum<TEnum>(SqlDataReader reader, string columnName, Action<TEnum> setter) where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var i = reader.GetOrdinal(columnName);

            TEnum result;

            if (!reader.IsDBNull(i) && Enum.TryParse(reader[columnName].ToString(), out result))
            {
                setter(result);
            }
        }

        private void TryParseDateTime(SqlDataReader reader, string columnName, Action<DateTime> setter)
        {
            var i = reader.GetOrdinal(columnName);

            DateTime value;

            if (!reader.IsDBNull(i) && DateTime.TryParse(reader[columnName].ToString(), out value))
            {
                setter(value);
            }
        }

        public IEnumerable<Note> FilterByCreationDate(int userId, DateTime date)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByCreationDateQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId),
                    new SqlParameter("@CreationDate", date));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Note> FilterByLastChangeDate(int userId, DateTime lastChangeDate)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByLastChangeDateQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId),
                    new SqlParameter("@LastChangeDate", lastChangeDate));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Note> FilterByNoteType(int userId, NoteType noteType)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByNoteTypeQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId),
                    new SqlParameter("@NoteType", noteType.ToString()));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Note> FilterByCurrentState(int userId, State state)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByStateQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId),
                    new SqlParameter("@State", state.ToString()));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Note> FilterByPriority(int userId, Priority priority)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByPriorityQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId), new SqlParameter("@Priority", priority.ToString()));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Note> FilterByCreationBetween(int userId, DateTime startLimit, DateTime endLimit)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByCreationBetweenQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId),
                    new SqlParameter("@StartLimit", startLimit),
                    new SqlParameter("@EndLimit", endLimit));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Note> FilterByStartDate(int userId, DateTime startDate)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByStartDateQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId),
                    new SqlParameter("@StartDate", startDate));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Note> FilterByEndDate(int userId, DateTime endDate)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByEndDateQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId),
                    new SqlParameter("@EndDate", endDate));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public override int Insert(Note entity, SqlTransaction sqlTransaction)
        {
            var query = NoteQueries.GetInsertQuery();
            return Insert(entity, query, sqlTransaction);
        }

        public override int Update(Note entity, SqlTransaction sqlTransaction)
        {
            var query = NoteQueries.GetUpdateQuery();
            return Update(entity, query, sqlTransaction);
        }

        public override int Delete(int id, SqlTransaction sqlTransaction)
        {
            var query = NoteQueries.GetDeleteQuery();
            return Delete(id, query, sqlTransaction);
        }

        public override Note GetById(int id)
        {
            var query = NoteQueries.GetGetByIdQuery();

            return GetById(id, query);
        }

        public override IEnumerable<Note> GetAll()
        {
            return GetAll(NoteQueries.GetAllQuery());
        }

        public IEnumerable<Note> FilterByCaptionLike(int userId, string caption)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByCaptionQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId),
                    new SqlParameter("@Caption", caption.MakeLikeExpression()));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public Note GetNoteByCaption(int userId, string caption)
        {
            Note result = null;

            var query = NoteQueries.GetNoteByCaptionQuery();

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, new SqlParameter("@UserId", userId),
                    new SqlParameter("@Caption", caption));

                using (var reader = cmd.ExecuteReader())
                {
                    result = Map(reader);
                }
            }

            return result;
        }

        public IEnumerable<Note> GetUserNotes(int userId)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetUserNotesQuery();

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

        public override int Count()
        {
            return Count(NoteQueries.NoteTable);
        }
    }
}