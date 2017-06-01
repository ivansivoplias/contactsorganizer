using Organizer.DAL.Entities;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.DAL.Repository
{
    public class ContactRepository : IRepository<Contact>
    {
        public void Create(Contact entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Contact entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Contact Get(int id)
        {
            throw new NotImplementedException();
        }

        public Contact Get(object key)
        {
            throw new NotImplementedException();
        }

        public ICollection<Contact> GetAll()
        {
            throw new NotImplementedException();
        }

        public ICollection<Contact> Select()
        {
            throw new NotImplementedException();
        }

        public void Update(Contact entity)
        {
            throw new NotImplementedException();
        }
    }
}