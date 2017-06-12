using System;
using System.Collections.Generic;
using Organizer.Infrastructure.Database;
using System.Data.SqlClient;
using Organizer.Common;
using Organizer.Common.Entities;

namespace Organizer.DAL.Repository
{
    public class NoteRepository : RepositoryBase<Note>
    {
        public NoteRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Passes the parameters for Insert Statement
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cmd"></param>
        protected override void InsertCommandParameters(Note entity, SqlCommand cmd)
        {
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

        /// <summary>
        /// Passes the parameters for Update Statement
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cmd"></param>
        protected override void UpdateCommandParameters(Note entity, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue($"@{entity.IdColumnName}", entity.Id);
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

        /// <summary>
        /// Passes the parameters to command for Delete Statement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cmd"></param>
        protected override void DeleteCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new Note().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        /// <summary>
        /// Passes the parameters to command for populate by key statement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cmd"></param>
        protected override void GetByIdCommandParameters(int id, SqlCommand cmd)
        {
            var idColumnName = new Note().IdColumnName;
            cmd.Parameters.AddWithValue($"@{idColumnName}", id);
        }

        /// <summary>
        /// Maps data for populate by key statement
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override Note Map(SqlDataReader reader)
        {
            var note = new Note();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    note.Id = Convert.ToInt32(reader[note.IdColumnName].ToString());

                    TryParseDateTime(reader, "CreationDate", (x) => note.CreationDate = x);

                    TryParseDateTime(reader, "LastChangeDate", (x) => note.LastChangeDate = x);

                    note.NoteText = reader["NoteText"].ToString();

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

        /// <summary>
        /// Maps data for populate all statement
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override List<Note> MapCollection(SqlDataReader reader)
        {
            var notes = new List<Note>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var note = new Note();
                    note.Id = Convert.ToInt32(reader[note.IdColumnName].ToString());

                    TryParseDateTime(reader, "CreationDate", (x) => note.CreationDate = x);

                    TryParseDateTime(reader, "LastChangeDate", (x) => note.LastChangeDate = x);

                    note.NoteText = reader["NoteText"].ToString();

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
    }
}