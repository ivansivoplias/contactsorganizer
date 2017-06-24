using System.Collections.Generic;
using Organizer.Common.DTO;

namespace Organizer.Infrastructure.Services
{
    public interface IContactService
    {
        void AddContact(ContactDto contact);

        void RemoveContact(ContactDto contact);

        void EditContact(ContactDto contact);

        ContactDto GetContact(int id);

        ICollection<ContactDto> GetContacts(UserDto user);

        ICollection<ContactDto> GetContactsByPhone(UserDto user, string phone);

        ICollection<ContactDto> GetContacsBySocialInfo(UserDto user, SocialInfoDto info);

        ICollection<ContactDto> GetContactsByFirstName(UserDto user, string firstName);

        ICollection<ContactDto> GetContactsByLastName(UserDto user, string lastName);

        ICollection<ContactDto> GetContactsByMiddleName(UserDto user, string middleName);

        ICollection<ContactDto> GetContactsByPersonalInfo(UserDto user, PersonalInfoDto info);

        ContactDto FindByNickName(UserDto user, string nickName);

        ContactDto FindByPrimaryPhone(UserDto user, string phone);

        ICollection<ContactDto> GetContactsByEmail(UserDto user, string email);
    }
}