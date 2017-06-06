using Organizer.DAL.Entities;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Organizer.Common;

namespace Organizer.DAL.Repository
{
    public class NoteRepository : RepositoryBase<Note>
    {
        public NoteRepository(IDbContext context) : base(context, "Notes")
        {
        }

        public override void Create(Note entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Note entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Note Get(int id)
        {
            throw new NotImplementedException();
        }

        public override Note Get(object key)
        {
            throw new NotImplementedException();
        }

        public override ICollection<Note> GetAll()
        {
            throw new NotImplementedException();
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

        public override ICollection<Note> Select()
        {
            throw new NotImplementedException();
        }

        public override void Update(Note entity)
        {
            throw new NotImplementedException();
        }
    }
}