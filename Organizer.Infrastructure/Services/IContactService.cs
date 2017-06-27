using Organizer.Common.DTO;
using System.Collections.Generic;

namespace Organizer.Infrastructure.Services
{
    public interface IContactService
    {
        void AddContact(ContactDto contact);

        void RemoveContact(ContactDto contact);

        void EditContact(ContactDto contact);

        ContactDto GetContact(int id);

        ICollection<ContactDto> GetContacts(UserDto user, int pageSize, int page);

        ICollection<ContactDto> GetContactsByPhone(UserDto user, string phone, int pageSize, int page);

        ICollection<ContactDto> GetContacsBySocialInfo(UserDto user, SocialInfoDto info, int pageSize, int page);

        ICollection<ContactDto> GetContactsByFirstName(UserDto user, string firstName, int pageSize, int page);

        ICollection<ContactDto> GetContactsByLastName(UserDto user, string lastName, int pageSize, int page);

        ICollection<ContactDto> GetContactsByMiddleName(UserDto user, string middleName, int pageSize, int page);

        ICollection<ContactDto> GetContactsByPersonalInfo(UserDto user, PersonalInfoDto info, int pageSize, int page);

        ContactDto FindByNickName(UserDto user, string nickName);

        ContactDto FindByPrimaryPhone(UserDto user, string phone);

        ICollection<ContactDto> GetContactsByEmail(UserDto user, string email, int pageSize, int page);
    }
}