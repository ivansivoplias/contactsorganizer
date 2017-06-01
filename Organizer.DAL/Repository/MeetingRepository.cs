using Organizer.DAL.Entities;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.DAL.Repository
{
    public class MeetingRepository : IRepository<Meeting>
    {
        public void Create(Meeting entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Meeting entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Meeting Get(int id)
        {
            throw new NotImplementedException();
        }

        public Meeting Get(object key)
        {
            throw new NotImplementedException();
        }

        public ICollection<Meeting> GetAll()
        {
            throw new NotImplementedException();
        }

        public ICollection<Meeting> Select()
        {
            throw new NotImplementedException();
        }

        public void Update(Meeting entity)
        {
            throw new NotImplementedException();
        }
    }
}