using Organizer.Common.DTO;
using Organizer.Common.Entities;
using System.Collections.Generic;

namespace Organizer.Infrastructure.Services
{
    public interface IContactService
    {
        void AddContact(ContactDto contact);

        void RemoveContact(ContactDto contact);

        void EditContact(ContactDto contact);

        ContactDto GetContact(int id);

        ICollection<ContactDto> GetContacts(User user, int pageSize, int page);

        int GetContactsCount(User user);

        ICollection<ContactDto> GetContactsByPhone(User user, string phone, int pageSize, int page);

        int GetContactsByPhoneCount(User user, string phone);

        ICollection<ContactDto> GetContacsBySocialInfo(User user, string appId, int pageSize, int page);

        int GetContactsBySocialInfoCount(User user, string appId);

        ICollection<ContactDto> GetContactsByFirstName(User user, string firstName, int pageSize, int page);

        int GetContactsByFirstNameCount(User user, string firstName);

        ICollection<ContactDto> GetContactsByLastName(User user, string lastName, int pageSize, int page);

        int GetContactsByLastNameCount(User user, string lastName);

        ICollection<ContactDto> GetContactsByMiddleName(User user, string middleName, int pageSize, int page);

        int GetContactsByMiddleNameCount(User user, string middleName);

        ICollection<ContactDto> GetContactsByPersonalInfo(User user, string personalInfo, int pageSize, int page);

        int GetContactsByPersonalInfoCount(User user, string personalInfo);

        ContactDto FindByNickName(User user, string nickName);

        ContactDto FindByPrimaryPhone(User user, string phone);

        ICollection<ContactDto> GetContactsByEmail(User user, string email, int pageSize, int page);

        int GetContactsByEmailCount(User user, string email);
    }
}