using Organizer.DAL.Entities;
using System.Collections.Generic;

namespace Organizer.BL.Abstract
{
    public interface IContactService
    {
        void AddContact(Contact contact);

        void RemoveContact(Contact contact);

        void EditContact(Contact contact);

        Contact GetContact(string phone);

        Contact GetContact(int id);

        ICollection<Contact> GetContacts(User user);
    }
}