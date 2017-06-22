using System.Collections.Generic;
using Organizer.Common.Entities;

namespace Organizer.Infrastructure.Services
{
    public interface IContactService
    {
        void AddContact(Contact contact);

        void RemoveContact(Contact contact);

        void EditContact(Contact contact);

        Contact GetContact(int id);

        ICollection<Contact> GetContacts(User user);

        ICollection<Contact> GetContactsByPhone(User user, string phone);

        ICollection<Contact> GetContacsBySocialInfo(User user, SocialInfo info);

        ICollection<Contact> GetContactsByFirstName(User user, string firstName);

        ICollection<Contact> GetContactsByLastName(User user, string lastName);

        ICollection<Contact> GetContactsByMiddleName(User user, string middleName);

        ICollection<Contact> GetContactsByPersonalInfo(User user, PersonalInfo info);

        Contact FindByNickName(User user, string nickName);

        Contact FindByPrimaryPhone(User user, string phone);

        ICollection<Contact> GetContactsByEmail(User user, string email);
    }
}