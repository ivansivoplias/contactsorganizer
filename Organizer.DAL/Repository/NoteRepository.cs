using Organizer.Common;
using Organizer.DAL.Entities;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace Organizer.DAL.Repository
{
    public class NoteRepository : RepositoryBase<Note>
    {
        private const string TableName = "Notes";
        private DataTable _notesTable;

        public NoteRepository(IDbContext context) : base(context, TableName)
        {
            _notesTable = _dataSet.Tables[TableName];
        }

        public override void Create(Note entity)
        {
            DataRow noteRow = _notesTable.NewRow();
            noteRow["NoteText"] = entity.NoteText;
            noteRow["NoteType"] = entity.NoteType.ToString();
            noteRow["CreationDate"] = entity.CreationDate;
            noteRow["LastChangeDate"] = entity.LastChangeDate;
            noteRow["UserId"] = entity.UserId;

            if (entity.State.HasValue)
                noteRow["State"] = entity.State.Value.ToString();

            if (entity.Priority.HasValue)
                noteRow["Priority"] = entity.Priority.Value.ToString();

            if (entity.StartDate.HasValue)
                noteRow["StartDate"] = entity.StartDate.Value;

            if (entity.EndDate.HasValue)
                noteRow["EndDate"] = entity.EndDate.Value;

            _notesTable.Rows.Add(noteRow);
        }

        public override void Delete(Note entity)
        {
            Delete(entity.Id);
        }

        public override void Delete(int id)
        {
            try
            {
                var datarow = _notesTable.Rows.Find(id);
                _notesTable.Rows.Remove(datarow);
            }
            catch (MissingPrimaryKeyException) { }
        }

        public override Note Get(int id)
        {
            Note result = null;
            try
            {
                var datarow = _notesTable.Rows.Find(id);
                result = Map(datarow);
            }
            catch (MissingPrimaryKeyException) { }

            return result;
        }

        public override Note Get(object key)
        {
            throw new NotImplementedException();
        }

        public override ICollection<Note> GetAll()
        {
            var result = new List<Note>();
            var dataRows = _notesTable.Select();
            if (dataRows != null && dataRows.Length > 0)
            {
                foreach (var row in dataRows)
                {
                    result.Add(Map(row));
                }
            }
            return result;
        }

        public override Note Map(IDataRecord record)
        {
            var note = new Note();
            note.Id = int.Parse(record["NoteId"] as string);
            note.NoteText = record["NoteText"] as string;
            note.CreationDate = DateTime.Parse(record["CreationDate"] as string);
            note.LastChangeDate = DateTime.Parse(record["LastChangeDate"] as string);
            note.NoteType = (NoteType)Enum.Parse(typeof(NoteType), record["NoteType"] as string);

            if (note.NoteType == NoteType.Todo)
            {
                note.State = (State)Enum.Parse(typeof(State), record["State"] as string);
                note.Priority = (Priority)Enum.Parse(typeof(Priority), record["Priority"] as string);
                note.StartDate = DateTime.Parse(record["StartDate"] as string);
                note.EndDate = DateTime.Parse(record["EndDate"] as string);
            }

            note.UserId = int.Parse(record["UserId"] as string);

            return note;
        }

        public override Note Map(DataRow row)
        {
            var note = new Note();
            note.Id = int.Parse(row["NoteId"] as string);
            note.NoteText = row["NoteText"] as string;
            note.CreationDate = DateTime.Parse(row["CreationDate"] as string);
            note.LastChangeDate = DateTime.Parse(row["LastChangeDate"] as string);
            note.NoteType = (NoteType)Enum.Parse(typeof(NoteType), row["NoteType"] as string);

            if (note.NoteType == NoteType.Todo)
            {
                note.State = (State)Enum.Parse(typeof(State), row["State"] as string);
                note.Priority = (Priority)Enum.Parse(typeof(Priority), row["Priority"] as string);
                note.StartDate = DateTime.Parse(row["StartDate"] as string);
                note.EndDate = DateTime.Parse(row["EndDate"] as string);
            }

            note.UserId = int.Parse(row["UserId"] as string);

            return note;
        }

        public override ICollection<Note> Select()
        {
            throw new NotImplementedException();
        }

        public override void Update(Note entity)
        {
            try
            {
                var noteRow = _notesTable.Rows.Find(entity.Id);
                noteRow.BeginEdit();
                noteRow["NoteText"] = entity.NoteText;
                noteRow["NoteType"] = entity.NoteType.ToString();
                noteRow["CreationDate"] = entity.CreationDate;
                noteRow["LastChangeDate"] = entity.LastChangeDate;
                noteRow["UserId"] = entity.UserId;

                if (entity.State.HasValue)
                    noteRow["State"] = entity.State.Value.ToString();

                if (entity.Priority.HasValue)
                    noteRow["Priority"] = entity.Priority.Value.ToString();

                if (entity.StartDate.HasValue)
                    noteRow["StartDate"] = entity.StartDate.Value;

                if (entity.EndDate.HasValue)
                    noteRow["EndDate"] = entity.EndDate.Value;
                noteRow.EndEdit();
            }
            catch (MissingPrimaryKeyException) { }
        }
    }
}