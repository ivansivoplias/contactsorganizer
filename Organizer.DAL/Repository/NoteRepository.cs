using Organizer.Common.Entities;
using Organizer.Common.Enums;
using Organizer.DAL.Helpers;
using Organizer.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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

        public IEnumerable<Note> FilterByCreationDate(int userId, DateTime date, int? pageSize = null, int? page = null)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByCreationDateQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("CreationDate", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, NoteParams.GetFilterByCreationDateParams(userId, date));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Note> FilterByLastChangeDate(int userId, DateTime lastChangeDate, int? pageSize = null, int? page = null)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByLastChangeDateQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("LastChangeDate", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, NoteParams.GetFilterByLastChangeDateParams(userId, lastChangeDate));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Note> FilterByNoteType(int userId, NoteType noteType, int? pageSize = null, int? page = null)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByNoteTypeQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("NoteType", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, NoteParams.GetFilterByNoteTypeParams(userId, noteType));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Note> FilterByCurrentState(int userId, State state, int? pageSize = null, int? page = null)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByStateQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("State", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, NoteParams.GetFilterByStateParams(userId, state));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Note> FilterByPriority(int userId, Priority priority, int? pageSize = null, int? page = null)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByPriorityQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("Priority", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, NoteParams.GetFilterByPriorityParams(userId, priority));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Note> FilterByCreationBetween(int userId, DateTime startLimit, DateTime endLimit, int? pageSize = null, int? page = null)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByCreationBetweenQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("CreationDate", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, NoteParams.GetFilterByCreationBetweenParams(userId, startLimit, endLimit));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Note> FilterByStartDate(int userId, DateTime startDate, int? pageSize = null, int? page = null)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByStartDateQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("StartDate", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, NoteParams.GetFilterByStartDateParams(userId, startDate));

                using (var reader = cmd.ExecuteReader())
                {
                    result = MapCollection(reader);
                }
            }

            return result;
        }

        public IEnumerable<Note> FilterByEndDate(int userId, DateTime endDate, int? pageSize = null, int? page = null)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByEndDateQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("EndDate", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, NoteParams.GetFilterByEndDateParams(userId, endDate));

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

        public IEnumerable<Note> FilterByCaptionLike(int userId, string caption, int? pageSize = null, int? page = null)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetFilterByCaptionQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("Caption", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, NoteParams.GetFilterByCaptionParams(userId, caption));

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

        public IEnumerable<Note> GetUserNotes(int userId, int? pageSize = null, int? page = null)
        {
            IEnumerable<Note> result = null;

            var query = NoteQueries.GetUserNotesQuery();

            if (pageSize != null && page != null)
            {
                query = query.AddPaging("Caption", pageSize.Value, page.Value);
            }

            using (var cmd = _connection.CreateCommand())
            {
                QueryHelper.SetupCommand(cmd, query, NoteParams.GetGetUserNotesParams(userId));

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