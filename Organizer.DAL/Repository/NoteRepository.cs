using Organizer.DAL.Entities;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.DAL.Repository
{
    public class NoteRepository : IRepository<Note>
    {
        public void Create(Note entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Note entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Note Get(int id)
        {
            throw new NotImplementedException();
        }

        public Note Get(object key)
        {
            throw new NotImplementedException();
        }

        public ICollection<Note> GetAll()
        {
            throw new NotImplementedException();
        }

        public ICollection<Note> Select()
        {
            throw new NotImplementedException();
        }

        public void Update(Note entity)
        {
            throw new NotImplementedException();
        }
    }
}