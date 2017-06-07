using Organizer.DAL.Entities;
using Organizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;

namespace Organizer.DAL.Repository
{
    public class ContactRepository : RepositoryBase<Contact>
    {
        private const string TableName = "Contacts";
        private DataTable _contactsTable;

        public ContactRepository(IDbContext context) : base(context, TableName)
        {
            _contactsTable = _dataSet.Tables[TableName];
        }

        public override void Create(Contact entity)
        {
            DataRow contactRow = _contactsTable.NewRow();
            contactRow["Phone"] = entity.Phone;
            contactRow["UserId"] = entity.UserId;
            _contactsTable.Rows.Add(contactRow);
        }

        public override void Delete(Contact entity)
        {
            Delete(entity.Id);
        }

        public override void Delete(int id)
        {
            try
            {
                var datarow = _contactsTable.Rows.Find(id);
                _contactsTable.Rows.Remove(datarow);
            }
            catch (MissingPrimaryKeyException) { }
        }

        public override Contact Get(int id)
        {
            Contact result = null;
            try
            {
                var datarow = _contactsTable.Rows.Find(id);
                result = Map(datarow);
            }
            catch (MissingPrimaryKeyException) { }

            return result;
        }

        public override Contact Get(object key)
        {
            throw new NotImplementedException();
        }

        public override ICollection<Contact> GetAll()
        {
            var result = new List<Contact>();
            var dataRows = _contactsTable.Select();
            if (dataRows != null && dataRows.Length > 0)
            {
                foreach (var row in dataRows)
                {
                    result.Add(Map(row));
                }
            }
            return result;
        }

        public override Contact Map(IDataRecord record)
        {
            var contact = new Contact();
            contact.Id = int.Parse(record["ContactId"] as string);
            contact.Phone = record["Phone"] as string;
            contact.UserId = int.Parse(record["UserId"] as string);

            return contact;
        }

        public override Contact Map(DataRow row)
        {
            var contact = new Contact();
            contact.Id = int.Parse(row["ContactId"] as string);
            contact.Phone = row["Phone"] as string;
            contact.UserId = int.Parse(row["UserId"] as string);

            return contact;
        }

        public override ICollection<Contact> Select()
        {
            throw new NotImplementedException();
        }

        public override void Update(Contact entity)
        {
            try
            {
                var row = _contactsTable.Rows.Find(entity.Id);
                row.BeginEdit();
                row["Phone"] = entity.Phone;
                row["UserId"] = entity.UserId;
                row.EndEdit();
            }
            catch (MissingPrimaryKeyException) { }
        }
    }
}