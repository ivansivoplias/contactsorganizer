using Organizer.DAL.Entities;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace Organizer.DAL.Repository
{
    public class ContactRepository : RepositoryBase<Contact>
    {
        public ContactRepository(IDbContext context) : base(context, "Contacts")
        {
        }

        public override void Create(Contact entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Contact entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Contact Get(int id)
        {
            throw new NotImplementedException();
        }

        public override Contact Get(object key)
        {
            throw new NotImplementedException();
        }

        public override ICollection<Contact> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Contact Map(IDataRecord record)
        {
            var contact = new Contact();
            contact.Id = int.Parse(record["ContactId"] as string);
            contact.Phone = record["Phone"] as string;
            contact.UserId = int.Parse(record["UserId"] as string);

            return contact;
        }

        public override ICollection<Contact> Select()
        {
            throw new NotImplementedException();
        }

        public override void Update(Contact entity)
        {
            throw new NotImplementedException();
        }
    }
}