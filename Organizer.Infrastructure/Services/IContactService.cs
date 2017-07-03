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

        ICollection<ContactDto> GetContactsByPhone(User user, string phone, int pageSize, int page);

        ICollection<ContactDto> GetContacsBySocialInfo(User user, string appId, int pageSize, int page);

        ICollection<ContactDto> GetContactsByFirstName(User user, string firstName, int pageSize, int page);

        ICollection<ContactDto> GetContactsByLastName(User user, string lastName, int pageSize, int page);

        ICollection<ContactDto> GetContactsByMiddleName(User user, string middleName, int pageSize, int page);

        ICollection<ContactDto> GetContactsByPersonalInfo(User user, string personalInfo, int pageSize, int page);

        ContactDto FindByNickName(User user, string nickName);

        ContactDto FindByPrimaryPhone(User user, string phone);

        ICollection<ContactDto> GetContactsByEmail(User user, string email, int pageSize, int page);
    }
}